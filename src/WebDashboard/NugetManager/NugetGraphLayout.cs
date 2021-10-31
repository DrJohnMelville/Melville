using GraphSharp.Controls;
using QuikGraph;

namespace WebDashboard.NugetManager;

public class NugetGraphLayout: GraphLayout<ProjectFile, ProjectEdge, ProjectGraph>
{
        
}

public class ProjectGraph: BidirectionalGraph<ProjectFile, ProjectEdge>
{
}

public class ProjectEdge: Edge<ProjectFile>
{
    public ProjectEdge(ProjectFile source, ProjectFile target) : base(source, target)
    {
    }
}