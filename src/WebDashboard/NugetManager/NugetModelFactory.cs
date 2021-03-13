using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Melville.MVVM.FileSystem;
using Melville.MVVM.Functional;

namespace WebDashboard.NugetManager
{
    public class NugetModelFactory
    {
        private List<ProjectFile> projects = new();
        public async Task<NugetModel> Create(IFile buildPropsFile)
        {
            await projects.AddRangeAsync(FindProjects(buildPropsFile));
            await ComputeProjectDependencies();
            return new(await buildPropsFile.GetUniqueTag("Version") ?? "No Version Found",
                projects);
        }
        
        private IAsyncEnumerable<ProjectFile> FindProjects(IFile buildPropsFile)
        {
            return ProjectFiles(
                    buildPropsFile.Directory ?? throw new InvalidOperationException("No Directory"))
                .Select(i => new ProjectFile(i));
        }

        private async IAsyncEnumerable<IFile> ProjectFiles(IDirectory directory)
        {
            foreach (var candidateDir in GetAllSubdirectories(directory))
            {
                if (await ValidProjectFile(candidateDir) is { } projFile)
                {
                    yield return projFile;
                }
            }
        }

        private async Task<IFile?> ValidProjectFile(IDirectory candidateDir)
        {
            var file = candidateDir.File(candidateDir.Name + ".csproj");
            return file.Exists() && await FileGeneratesPackage(file) ? file : null;
        }

        private static async Task<bool> FileGeneratesPackage(IFile file) =>
            (await file.GetUniqueTag("GeneratePackageOnBuild"))?.ToLower().Equals("true") ?? false;

        private IEnumerable<IDirectory> GetAllSubdirectories(IDirectory directory)
            => directory.AllSubDirectories().SelectRecursive(i=>i.AllSubDirectories());
        
        private async Task ComputeProjectDependencies()
        {
            foreach (var file in projects)
            {
                var projFile = (await file.File.ReadAsXmlAsync()) as XElement;
                if (projFile == null) continue;
                foreach (var dependency in projFile
                    .Descendants("ProjectReference")
                    .Select(i=>i.Attribute("Include")?.Value)
                    .OfType<string>()
                    .Select(i=>PickProject(file.File.FileAtRelativePath(i)))
                    .OfType<ProjectFile>())
                {
                        file.DependsOn.Add(dependency);
                }
            }
        }

        private ProjectFile? PickProject(IFile? projFile) => 
            projFile == null ?
                null : 
                projects.FirstOrDefault(i => i.File.Path.Equals(projFile.Path));
    }
}