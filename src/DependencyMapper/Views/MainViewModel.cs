using DependencyMapper.Model;
using DependencyMapper.ModelFactory;
using Melville.FileSystem;
using Melville.Hacks;
using Melville.Lists;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;

namespace DependencyMapper.Views;

public class MainViewModel
{
    public IList<Dependency> Roots { get; } = new ThreadSafeBindableCollection<Dependency>();
    public IList<Dependency> Leaves { get; } = new ThreadSafeBindableCollection<Dependency>();

    public async Task AddProject([FromServices] IOpenSaveFile osf)
    {
        var file = osf.GetLoadFile(null, ".csproj",
            "Project File (*.csprog)|*.csproj", "Open a project");
        if (file == null) return;

        Roots.Add(await new ProjectReader(Leaves, NugetPackageFolder()).ReadProject(file));
        var sorted = Leaves.OrderBy(i => i.Title).ToList();
        Leaves.Clear();
        Leaves.AddRange(sorted);
    }

    public IDirectory NugetPackageFolder() =>
        new FileSystemDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.nuget\packages");

}