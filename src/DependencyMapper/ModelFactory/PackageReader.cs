using System.Windows.Forms;
using System.Xml.Linq;
using DependencyMapper.Model;
using Melville.FileSystem;
using Melville.INPC;

namespace DependencyMapper.ModelFactory;

[FromConstructor]
public partial class InnerPackageReader : PackageReader
{
    protected override PackageReader CreateInnerReader() => this;
}

public class PackageReader(
    XName tagName,
    string nameAttribute,
    IPackageVersionReader versionAttribute,
    IDirectory nugetBase,
    IList<Dependency> dependencies)
{
    private PackageReader? innerReader = null;
    public PackageReader InnerReader => innerReader ??= CreateInnerReader();
    private static readonly XNamespace PackageNamespace =
        XNamespace.Get("http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd");

    protected virtual PackageReader CreateInnerReader() =>
        new InnerPackageReader(
            PackageNamespace+"dependency", "id", new AttributeVersionReader("version"), nugetBase, dependencies);

    public async Task LoadReferencedPackages(XElement source, Dependency parent)
    {
        foreach (var (name, version) in GetDependantPackages(source))
        {
            parent.AddDependency(await ReadPackage(name, version));
        }
    }

    private IEnumerable<(string, string)> GetDependantPackages(XElement project) =>
        project.Descendants(tagName)
            .Select(Read2AttributeNode)
            .Where(i=>i.HasValue)
            .Select(i=>i!.Value)
            .Distinct();

    private (string name, string version)? Read2AttributeNode(XElement arg) =>
        arg.Attribute(nameAttribute)?.Value is { } title &&
        versionAttribute.VersionFor(arg, title) is { } version
            ? (title, version)
            : null;

    private async ValueTask<Dependency> ReadPackage(string name, string version)
    {
        var packageName = $"{name} ({version})";
        if (SearchForPackage(packageName) is { } package)
            return package;
        var ret = new Dependency(packageName);
        dependencies.Add(ret);
        await ReadPackageFile(NuSpecFile(name, version), ret);
        return ret;
    }

    private Dependency? SearchForPackage(string packageName)
    {
        return dependencies.FirstOrDefault(i => i.Title.Equals(packageName, StringComparison.Ordinal));
    }

    private IFile NuSpecFile(string name, string version) =>
        nugetBase.SubDirectory(name).SubDirectory(version)
            .File($"{name}.nuspec");

    private async Task ReadPackageFile(IFile file, Dependency project)
    {
        if (!file.Exists()) return;
        var xml = (XElement)await file.ReadAsXmlAsync();
        await InnerReader.LoadReferencedPackages(xml, project);
    }
}