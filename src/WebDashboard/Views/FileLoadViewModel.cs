using System;
using System.Threading.Tasks;
using Melville.MVVM.FileSystem;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RootWindows;
using Melville.MVVM.Wpf.ViewFrames;
using Melville.WpfIocMvvm.StartupBases;
using WebDashboard.Models;
using WebDashboard.Startup;

namespace WebDashboard.Views
{
    [OnDisplayed(nameof(Setup))]
    public class FileLoadViewModel
    {
        private readonly IOpenSaveFile fileDlg;
        private readonly IStartupData startup;
        private readonly IRootModelFactory modelFactory;
        private readonly Func<RootModel, RootViewModel> viewModelFactory;
        private readonly INavigationWindow navigation;

        public FileLoadViewModel(IOpenSaveFile fileDlg, IStartupData startup, IRootModelFactory modelFactory, 
            Func<RootModel, RootViewModel> viewModelFactory, INavigationWindow navigation)
        {
            this.fileDlg = fileDlg;
            this.startup = startup;
            this.modelFactory = modelFactory;
            this.viewModelFactory = viewModelFactory;
            this.navigation = navigation;
        }

        public async Task Setup()
        {
            var file = GetPubXmlFile();
            if (file == null || !file.Exists()) return;
            navigation.NavigateTo(viewModelFactory(await modelFactory.Create(file)));
        }

        private IFile? GetPubXmlFile()
        {
            return HasPublishFileOnCommandLine()?startup.ArgumentAsFile(0):
                fileDlg.GetLoadFile(null, "pubxml", "Deploy File (*.pubxml)|*.pubxml", "Pick a Deploy file");
        }

        private bool HasPublishFileOnCommandLine() => 
            startup.CommandLineArguments.Length > 0 && 
            startup.CommandLineArguments[0].EndsWith(".pubxml", StringComparison.OrdinalIgnoreCase) &&
            startup.ArgumentAsFile(0).Exists();
    }
}