using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.FileSystem;
using Melville.MVVM.RunShellCommands;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.RootWindows;
using Melville.MVVM.Wpf.ViewFrames;


namespace WebDashboard.Views
{
    [OnDisplayed(nameof(RunDeployment))]
    public class DeploymentViewModel
    {
        public IRootViewModel Model { get; }
        public IList<string> Output { get; } = new ThreadSafeBindableCollection<string>();
        private readonly INavigationWindow navigator;
        private readonly IHasPassword password;

        public DeploymentViewModel(INavigationWindow navigator, IRootViewModel model, IHasPassword password)
        {
            this.navigator = navigator;
            Model = model;
            this.password = password;
        }

        public async Task RunDeployment([FromServices]IRunShellCommand runner)
        {
            var webConfig = Model.ProjectFile().Directory?.File("web.config") ??
                throw new InvalidOperationException("Cannot find project directory.");
            await using var preservedFile = new PreserveFile(webConfig);
            await PublishWithCustomWebConfig(runner, webConfig);
        }

        private async Task PublishWithCustomWebConfig(IRunShellCommand runner, IFile webConfig)
        {
            await WriteWebConfig(webConfig);
            await ExecutePublishProcess(runner);
            webConfig.Delete();
        }

        private async Task WriteWebConfig(IFile webConfig)
        {
            using var outputStream = await webConfig.CreateWrite();
            outputStream.Write(Encoding.UTF8.GetBytes(Model.WebConfig));
        }

        private async Task ExecutePublishProcess(IRunShellCommand runner)
        {
            var process = runner.CapturedShellExecute("dotnet", PublishSyntax());
            await process.PumpOutput(Output.Add);
        }

        private string PublishSyntax() => 
            $"publish {Model.ProjectFile().Path} /p:PublishProfile={Model?.PublishFile()?.NameWithoutExtension()} /p:Password={password.Password()}";

        public void Return() => navigator.NavigateTo(Model);
    }
}