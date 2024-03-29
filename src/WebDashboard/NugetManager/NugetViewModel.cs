﻿using System.Linq;
using Melville.FileSystem;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.RootWindows;
using WebDashboard.ConsoleWindows;

namespace WebDashboard.NugetManager;

public interface INugetViewModel
{
}

public class NugetViewModel: INugetViewModel
{
    public NugetModel Model { get; }

    public NugetViewModel(NugetModel model)
    {
        Model = model;
    }
        
    public void UploadPackages(INavigationWindow nav, [FromServices] IPackagePublishOperation uploadCommands)
    {
        var files = Model.Files
            .Where(i => i.Deploy)
            .Select(i => i.Package(Model.Version))
            .OfType<IFile>()
            .ToList();
        nav.NavigateTo( new ConsoleWindowViewModel(new NugetDeploymentCommands(this, files, uploadCommands)));
    }

}