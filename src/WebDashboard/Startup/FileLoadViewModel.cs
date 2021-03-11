using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melville.MVVM.FileSystem;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.EventBindings.SearchTree;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RootWindows;
using Melville.MVVM.Wpf.ViewFrames;
using WebDashboard.NugetManager;
using WebDashboard.SecretManager.Models;
using WebDashboard.SecretManager.Views;

namespace WebDashboard.Startup
{
    [OnDisplayed(nameof(Setup))]
    public class FileLoadViewModel
    {
        private readonly IOpenSaveFile fileDlg;
        private readonly IStartupData startup;
        private readonly INavigationWindow navigation;

        public FileLoadViewModel(IOpenSaveFile fileDlg, IStartupData startup, IRootModelFactory modelFactory, 
            Func<RootModel, RootViewModel> viewModelFactory, INavigationWindow navigation)
        {
            this.fileDlg = fileDlg;
            this.startup = startup;
            this.navigation = navigation;
        }

        public async Task Setup([FromServices]IList<IFileViewerFactory> factories)
        {
            var file = GetPubXmlFile();
            if (file == null || !file.Exists()) return;
            navigation.NavigateTo(await ViewModelForFile(factories, file));
        }

        private Task<object> ViewModelForFile(IList<IFileViewerFactory> factories, IFile file)
        {
            var extension = file.Extension().ToLower();
            return factories.First(i => i.IsValidForExtension(extension)).Create(file);
        }

        private IFile? GetPubXmlFile()
        {
            return HasPublishFileOnCommandLine()?startup.ArgumentAsFile(0):
                fileDlg.GetLoadFile(null, "pubxml", "Project or Deploy File|*.pubxml;*.csproj;*.props", "Pick a Deploy file");
        }

        private bool HasPublishFileOnCommandLine() => 
            startup.CommandLineArguments.Length > 0 && 
            startup.CommandLineArguments[0].EndsWith(".pubxml", StringComparison.OrdinalIgnoreCase) &&
            startup.ArgumentAsFile(0).Exists();
    }
}