using System.Collections.Generic;
using Melville.INPC;
using Melville.MVVM.FileSystem;

namespace WebDashboard.NugetManager
{
    public partial class ProjectFile
    {
        public IFile File { get; }
        public List<ProjectFile> DependsOn { get; } = new();
        [AutoNotify] private bool deploy;

        partial void WhenDeployChanges(bool oldValue, bool newValue)
        {
            foreach (var proj in DependsOn)
            {
                proj.Deploy = Deploy;
            }
        }
        
        public ProjectFile(IFile file)
        {
            this.File = file;
        }
    } 
    public class NugetModel
    {
        public string Version { get; }
        public IList<ProjectFile> Files { get; }
        public ProjectGraph Graph { get; }

        public NugetModel(string version, IList<ProjectFile> files)
        {
            Version = version;
            Files = files;
            Graph = new ProjectGraph();
            foreach (var vertex in Files)
            {
                Graph.AddVertex(vertex);
            }

            foreach (var source in Files)
            {
                foreach (var target in source.DependsOn)
                {
                    Graph.AddEdge(new ProjectEdge(source, target));
                }
            }
        }
    }
}