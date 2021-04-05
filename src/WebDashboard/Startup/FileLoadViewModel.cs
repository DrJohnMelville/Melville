using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melville.FileSystem.FileSystem;
using Melville.MVVM.Wpf.EventBindings.SearchTree;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RootWindows;
using Melville.MVVM.Wpf.ViewFrames;
using WebDashboard.NugetManager;

namespace WebDashboard.Startup
{
    [OnDisplayed(nameof(Setup))]
    public class FileLoadViewModel
    {
        private readonly IOpenSaveFile fileDlg;
        private readonly IStartupData startup;
        private readonly INavigationWindow navigation;
        private readonly IList<IFileViewerFactory> factories;

        public FileLoadViewModel(
            IOpenSaveFile fileDlg, IStartupData startup, INavigationWindow navigation,
            IList<IFileViewerFactory> factories)
        {
            this.fileDlg = fileDlg;
            this.startup = startup;
            this.navigation = navigation;
            this.factories = factories;
        }

        public async Task Setup()
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