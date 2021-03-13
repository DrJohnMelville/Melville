using System.Collections.Generic;
using Melville.MVVM.FileSystem;

namespace WebDashboard.NugetManager
{
    public class ProjectFile
    {
        public IFile File { get; }
        public List<ProjectFile> DependsOn { get; } = new();
        public ProjectFile(IFile file)
        {
            this.File = file;
        }
    } 
    public class NugetModel
    {
        public string Version { get; }
        public IList<ProjectFile> Files { get; }

        public NugetModel(string version, IList<ProjectFile> files)
        {
            Version = version;
            Files = files;
        }
    }
}