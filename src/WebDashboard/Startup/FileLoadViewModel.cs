using System;
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

        public async Task Setup(IVisualTreeRunner runner)
        {
            var file = GetPubXmlFile();
            if (file == null || !file.Exists()) return;
            var newVM = await ViewModelForFile(file, runner);
            navigation.NavigateTo(newVM);
        }

        private Task<object> ViewModelForFile(IFile file, IVisualTreeRunner runner) =>
            runner.RunOnTarget<Task<object>>(this, FactoryName(file.Extension().ToLower()), file);

        private string FactoryName(string extension) => extension switch
        {
            "props" => nameof(NugetManagerView),
            _ => nameof(SecretManagerView)
        };

        public Task<object> NugetManagerView(IFile file,
            [FromServices] Func<IFile, NugetViewModel> vmFactory) =>
            Task.FromResult<object>(vmFactory(file));
        
        public async Task<object> SecretManagerView(IFile file, 
            [FromServices] Func<RootModel, RootViewModel> factory, 
            [FromServices] IRootModelFactory rootModelFactory)
        {
            return factory(await rootModelFactory.Create(file));
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