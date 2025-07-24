using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Melville.FileSystem;

namespace WebDashboard.SecretManager.Models;

public class RootModel
{
    public PublishFileHolder? PublishFile { get; }
    public ProjectFileHolder ProjectFile { get; }
    public SecretFileHolder? RootSecretFile { get; }
    public SecretFileHolder? DeploymentSecretFile { get; }

    public bool SuppressWebDavModule { get; set; } = true;
    public bool DevelopmentMode { get; set; } 

    public RootModel(PublishFileHolder? publishFile, ProjectFileHolder projectFile, SecretFileHolder? rootSecretFile, 
        SecretFileHolder? deploymentSecretFile)
    {
        PublishFile = publishFile;
        ProjectFile = projectFile;
        RootSecretFile = rootSecretFile;
        DeploymentSecretFile = deploymentSecretFile;
    }

    public string ComputeWebConfig() => 
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n"+
        new XElement("configuration",
            new XElement("location",
                new XAttribute("path", "."),
                new XAttribute("inheritInChildApplications", false),
                TryAddCustomErrors(),
                new XElement("system.webServer",
                    WebDavSupression()!, // this acutally works
                    new XElement("handlers",
                        new XElement("add",
                            new XAttribute("name", "aspNetCore"),
                            new XAttribute("path", "*"),
                            new XAttribute("verb", "*"),
                            new XAttribute("modules", "AspNetCoreModuleV2"),
                            new XAttribute("resourceType", "Unspecified")
                        )
                    ),
                    new XElement("aspNetCore",
                        new XAttribute("processPath", $".\\{ProjectFile.File.NameWithoutExtension()}.exe"),
                        new XAttribute("stdoutLogEnabled", "false"),
                        new XAttribute("stdoutLogFile", ".\\logs\\stdout"),
                        new XAttribute("hostingModel", "inprocess"),
                        new XElement("environmentVariables", SecretsToEnviornment())
                    ),
                    TryAddHttpErrors()
                )
            )
        );

    private XElement? TryAddCustomErrors() =>
        DevelopmentMode
            ? new XElement("system.web",
                new XElement("customErrors",
                    new XAttribute("mode", "Off")))
            : null;
    private XElement? TryAddHttpErrors() =>
        DevelopmentMode
            ?   new XElement("httpErrors",
                    new XAttribute("mode", "Detailed"))
            : null;

    private XElement? WebDavSupression() =>
        SuppressWebDavModule?
            new XElement("modules",
                new XAttribute("runAllManagedModulesForAllRequests", "false"),
                new XElement("remove", new XAttribute("name", "WebDAVModule"))
            ): null;

    private IEnumerable<XElement> SecretsToEnviornment()
    {
        var dict = new Dictionary<string, string>();
        RootSecretFile?.AddSecrets(dict);
        DeploymentSecretFile?.AddSecrets(dict);
        AddDevelopmentIfRequested(dict);
        return FormatAsEnviornmentXmlElements(dict);
    }

    private void AddDevelopmentIfRequested(Dictionary<string, string> dict)
    {
        if (DevelopmentMode)
        {
            dict["ASPNETCORE_ENVIRONMENT"] = "Development";
        }
    }

    private static IEnumerable<XElement> FormatAsEnviornmentXmlElements(Dictionary<string, string> dict) =>
        dict.Select(i => new XElement("environmentVariable",
            new XAttribute("name", i.Key),
            new XAttribute("value", i.Value)
        ));
}

public abstract class XmlFileHolder
{
    public IFile File { get; }
    protected XElement Xml;

    protected XmlFileHolder(IFile file, XElement xml)
    {
        File = file;
        Xml = xml;
    }
}
    
public sealed class PublishFileHolder: XmlFileHolder
{
    private static readonly XNamespace msBuildNamespace =
        XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");

    public PublishFileHolder(IFile file, XElement xml) : base(file, xml)
    {
    }
    public string DeployedPath => Xml.Descendants(msBuildNamespace+"DeployIisAppPath")
        .FirstOrDefault()?.Value ??"No Path Found";

    public string UserSecretId => 
        Xml.Descendants(msBuildNamespace+"UserSecretsId").Concat(Xml.Descendants("UserSecretsId"))
        .FirstOrDefault()?.Value ?? "";
}

public sealed class ProjectFileHolder : XmlFileHolder
{
    public ProjectFileHolder(IFile file, XElement xml) : base(file, xml)
    {
    }

    public string UserSecretId => Xml.Descendants("UserSecretsId").FirstOrDefault()?.Value ?? ""; 
}