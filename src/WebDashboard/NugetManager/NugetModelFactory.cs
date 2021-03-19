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
            ComputeProjectDependencies();
            return new(await buildPropsFile.GetUniqueTag("Version") ?? "No Version Found",
                projects);
        }
        
        private IAsyncEnumerable<ProjectFile> FindProjects(IFile buildPropsFile)
        {
            return ProjectFiles(buildPropsFile.Directory ?? throw new InvalidOperationException("No Directory"));
        }

        private async IAsyncEnumerable<ProjectFile> ProjectFiles(IDirectory directory)
        {
            foreach (var candidateDir in GetAllSubdirectories(directory))
            {
                if (await ValidProjectFile(candidateDir) is { GeneratesPackageOnBuild: true} projFile)
                {
                    yield return projFile;
                }
            }
        }

        private async Task<ProjectFile?> ValidProjectFile(IDirectory candidateDir)
        {
            var file = candidateDir.File(candidateDir.Name + ".csproj");
            return file.Exists() && (await file.ReadAsXmlAsync()) is XElement elt 
                   ? new ProjectFile(file, elt) : null;
        }
        
        private IEnumerable<IDirectory> GetAllSubdirectories(IDirectory directory)
            => VisibileSubDirs(directory).SelectRecursive(VisibileSubDirs);

        private IEnumerable<IDirectory> VisibileSubDirs(IDirectory i)
        {
            return i.AllSubDirectories().Where(j => !j.Name.StartsWith("."));
        }

        private void ComputeProjectDependencies()
        {
            foreach (var file in projects)
            {
                foreach (var dependency in file.ComputeDependencies()
                    .Select(PickProject)
                    .OfType<ProjectFile>())
                {
                        file.DependsOn.Add(dependency);
                }
            }
        }

        private ProjectFile? PickProject(IFile projFile) => 
                projects.FirstOrDefault(i => i.File.Path.Equals(projFile.Path));
    }
}