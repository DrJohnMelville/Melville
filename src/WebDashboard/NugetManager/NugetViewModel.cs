using System.Linq;
using Melville.MVVM.FileSystem;
using Melville.MVVM.Wpf.RootWindows;
using WebDashboard.ConsoleWindows;

namespace WebDashboard.NugetManager
{
    public interface INugetViewModel
    {
    }

    public class NugetViewModel: INugetViewModel
    {
        private readonly NugetModel model;

        public NugetViewModel(NugetModel model)
        {
            this.model = model;
        }
        
        public void UploadPackages(INavigationWindow nav)
        {
            var files = model.Files
                .Where(i => i.Deploy)
                .Select(i => i.Package(model.Version))
                .OfType<IFile>()
                .ToList();
            nav.NavigateTo( new ConsoleWindowViewModel(new NugetDeploymentCommands(this, files)));
        }

    }
}