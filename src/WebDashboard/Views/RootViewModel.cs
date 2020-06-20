using System;
using Windows.ApplicationModel.Store;
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
        IFile? PublishFile();
    }

    public class RootViewModel : NotifyBase, IRootViewModel
    {
        private ISecretFileEditorViewModel? projectSecrets = null;
        public ISecretFileEditorViewModel? ProjectSecrets
        {
            get => projectSecrets;
            set => AssignAndNotify(ref projectSecrets, value);
        }

        private ISecretFileEditorViewModel? deploymentSecrets = null;
        public ISecretFileEditorViewModel? DeploymentSecrets
        {
            get => deploymentSecrets;
            set => AssignAndNotify(ref deploymentSecrets, value);
        }

        public RootModel Model { get; }

        public RootViewModel(RootModel model)
        {
            Model = model;
            projectSecrets = CreateSecretEditor(model.RootSecretFile);
            deploymentSecrets = CreateSecretEditor(model.DeploymentSecretFile);
        }

        private static SecretFileEditorViewModel? CreateSecretEditor(SecretFileHolder? secretFile) => 
            secretFile==null?null: new SecretFileEditorViewModel(secretFile);

        public void UpdateWebConfig() => WebConfig = Model.ComputeWebConfig();

        private string webConfig = "";

        public string WebConfig
        {
            get => webConfig;
            set => AssignAndNotify(ref webConfig, value);
        }

        public IFile ProjectFile() => Model.ProjectFile.File;
        public IFile? PublishFile() => Model.PublishFile?.File;

        public void Deploy([FromServices] Func<RootViewModel, IHasPassword, DeploymentViewModel> createDeployment,
            INavigationWindow navigationWindow, IHasPassword password) =>
            navigationWindow.NavigateTo(createDeployment(this, password));

        public void SwapView(ISecretFileEditorViewModel view)
        {
            if (ProjectSecrets == view)
            {
                ProjectSecrets = view.CreateSwappedView();
            }
            if (DeploymentSecrets == view)
            {
                DeploymentSecrets = view.CreateSwappedView();
            }
        }
    }
}