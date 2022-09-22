using System.Collections.Generic;
using System.Linq;
using Melville.FileSystem;
using Melville.INPC;
using WebDashboard.ConsoleWindows;

namespace WebDashboard.NugetManager;


public partial class NugetDeploymentCommands: IConsoleCommands
{
    [FromConstructor] private readonly INugetViewModel model;
    [FromConstructor] private readonly IList<IFile> packages;
    [FromConstructor] private readonly IPackagePublishOperation pushOperation;

    partial void OnConstructed()
    {
        ;
    }

    public IAsyncEnumerable<(string Command, string Param)> Commands() => 
        packages.Select(pushOperation.MakeCommand).ToAsyncEnumerable();

    public object NavigateOnReturn() => model;
}