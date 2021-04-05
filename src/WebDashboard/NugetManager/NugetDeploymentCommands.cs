using System;
using System.Collections.Generic;
using System.Linq;
using Melville.FileSystem;
using WebDashboard.ConsoleWindows;

namespace WebDashboard.NugetManager
{
    public class NugetDeploymentCommands: IConsoleCommands
    {
        private readonly INugetViewModel model;
        private readonly IList<IFile> packages;

        public NugetDeploymentCommands(INugetViewModel model, IList<IFile> packages)
        {
            this.model = model;
            this.packages = packages;
        }

        public IAsyncEnumerable<(string Command, string Param)> Commands() => 
            packages.Select(i => ("gpr", $"push \"{i.Path}\"")).ToAsyncEnumerable();

        public object NavigateOnReturn() => model;
    }
}