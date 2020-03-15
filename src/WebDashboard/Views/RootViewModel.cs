using System;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.FileSystem;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.RootWindows;
using WebDashboard.Models;

namespace WebDashboard.Views
{
    public interface IRootViewModel
    {
        string WebConfig { get; }
        IFile ProjectFile();
        IFile PublishFile();
    }
    public class RootViewModel: NotifyBase, IRootViewModel
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public SecretFileEditorViewModel ProjectSecrets { get; }
        public SecretFileEditorViewModel DeploymentSecrets { get; }
        public RootModel Model { get; }

        public RootViewModel(RootModel model)
        {
            Model = model;
            ProjectSecrets = new SecretFileEditorViewModel(model.RootSecretFile);
            DeploymentSecrets = new SecretFileEditorViewModel(model.DeploymentSecretFile);
        }

        public void UpdateWebConfig() => WebConfig = Model.ComputeWebConfig();

        private string webConfig = "";
        public string WebConfig
        {
            get => webConfig;
            set => AssignAndNotify(ref webConfig, value);
        }

        public IFile ProjectFile() => Model.ProjectFile.File;
        public IFile PublishFile() => Model.PublishFile.File;

        public void Deploy([FromServices]Func<RootViewModel, IHasPassword, DeploymentViewModel> createDeployment,
            INavigationWindow navigationWindow, IHasPassword password)
        {
            navigationWindow.NavigateTo(createDeployment(this, password));
        }
    }
}