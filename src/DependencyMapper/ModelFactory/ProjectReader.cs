using System.Xml.Linq;
using DependencyMapper.Model;
using Melville.FileSystem;

namespace DependencyMapper.ModelFactory;

public readonly partial struct ProjectReader(
    IList<Dependency> dependencies, IDirectory nugetBase)
{
    private readonly PackageReader readFromProject = new(
        "PackageReference", "Include", "Version", nugetBase, dependencies);

    public async ValueTask<Dependency> ReadProject(IFile projectFile)
    {
        var projectName = projectFile.NameWithoutExtension();
        try
        {
            if (PriorProject(projectName) is { } item) return item;
            var newProj = new Dependency(projectName);
            dependencies.Add(newProj);
            await ParseProjectXml(projectFile, newProj);
            return newProj;
        }
        catch (Exception)
        {
            return new Dependency(projectName + " (Cannot Read)");
        }
    }

    private Dependency? PriorProject(string projectName)
    {
        return dependencies.OfType<Dependency>()
            .FirstOrDefault(i => i.Title.Equals(projectName));
    }

    private async Task ParseProjectXml(IFile projectFile, Dependency ret)
    {
        var project = (XElement)await projectFile.ReadAsXmlAsync();

        foreach (var node in GetDependantProjects(project))
        {
            ret.AddDependency(await ReadProject(projectFile.FileAtRelativePath(node)!));
        }

        await readFromProject.LoadReferencedPackages(project, ret);
    }

    private static IEnumerable<string> GetDependantProjects(XElement project) =>
        project.Descendants("ProjectReference")
            .Select(i => i.Attribute("Include")?.Value)
            .OfType<string>();

}