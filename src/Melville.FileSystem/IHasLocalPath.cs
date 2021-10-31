using System.Threading.Tasks;

namespace Melville.FileSystem;

public interface IHasLocalPath
{
  LocalFileNamePackage? LocalPath();
}

public class LocalFileNamePackage
{
  public string Name { get; }
  public Task Task { get; }

  public LocalFileNamePackage(string name) : this(name, Task.FromResult(1))
  {
  }
  public LocalFileNamePackage(string name, Task task)
  {
    Name = name;
    Task = task;
  }
}