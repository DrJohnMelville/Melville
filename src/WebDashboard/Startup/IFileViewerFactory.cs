using System;
using System.Threading.Tasks;
using Melville.MVVM.FileSystem;
using WebDashboard.NugetManager;
using WebDashboard.SecretManager.Models;
using WebDashboard.SecretManager.Views;

namespace WebDashboard.Startup
{
    public interface IFileViewerFactory
    {
        bool IsValidForExtension(string extension);
        Task<object> Create(IFile file);
    }
    public class NugetManagerViewModelFactory: IFileViewerFactory
    {
        private readonly Func<NugetViewModel> viewModelFactory;

        public NugetManagerViewModelFactory(Func<NugetViewModel> viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
        }

        public bool IsValidForExtension(string extension) =>
            extension.Equals("props", StringComparison.Ordinal);

        public Task<object> Create(IFile file) => Task.FromResult((object) viewModelFactory());

    }
    public class SecretManagerViewModelFactory: IFileViewerFactory
    {
        private readonly IRootModelFactory modelFactory;
        private readonly Func<RootModel, RootViewModel> viewModelFactory;

        public SecretManagerViewModelFactory(IRootModelFactory modelFactory, Func<RootModel, RootViewModel> viewModelFactory)
        {
            this.modelFactory = modelFactory;
            this.viewModelFactory = viewModelFactory;
        }

        public bool IsValidForExtension(string extension) => true;
        public async Task<object> Create(IFile file) =>
            viewModelFactory(await modelFactory.Create(file));
    }
}