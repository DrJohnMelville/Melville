using Melville.FileSystem;
using Microsoft.Extensions.Configuration;
using WebDashboard.Startup;

namespace WebDashboard.NugetManager;

public interface IPackagePublishOperation
{
    (string command, string param) MakeCommand(IFile packageFile);
}

public class GitHubPushOperation : IPackagePublishOperation
{
    public (string command, string param) MakeCommand(IFile packageFile) =>
        ("gpr", $"push \"{packageFile.Path}\"");
}

public partial class NugetPublishOperation : IPackagePublishOperation
{
    private readonly string? nugetKey;

    public NugetPublishOperation(IConfigurationRoot config)
    {
        nugetKey = config.GetValue<string>("NugetKey");
    }
    
    public (string command, string param) MakeCommand(IFile packageFile) => 
        nugetKey is null ?
            ("echo", "Count not find a nuget key."):
        ("dotnet", $"nuget push \"{packageFile.Path}\" --api-key {nugetKey} -s https://api.nuget.org/v3/index.json");
    
}