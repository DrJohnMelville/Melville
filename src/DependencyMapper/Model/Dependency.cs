using System.Collections.Specialized;
using Melville.INPC;
using Melville.Lists;

namespace DependencyMapper.Model;

public partial class Dependency
{
    [FromConstructor] public string Title { get; }

    public IList<Dependency> Dependencies { get; } = new ThreadSafeBindableCollection<Dependency>();
    public IList<Dependency> Users { get; } = new ThreadSafeBindableCollection<Dependency>();

    public void AddDependency(Dependency child)
    {
        Dependencies.Add(child);
        child.Users.Add(this);
    }
}
