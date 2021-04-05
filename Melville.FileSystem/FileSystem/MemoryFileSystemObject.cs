using System;
using System.IO;

namespace Melville.FileSystem.FileSystem
{
  public abstract class MemoryFileSystemObject : IFileSystemObject
  {
    public string Path { get; private set; }
    public string Name => System.IO.Path.GetFileName(Path);
    public abstract bool Exists();
    public FileAttributes Attributes { get; protected set; }
    public DateTime LastAccess { get; set; }
    public DateTime LastWrite { get; set; }
    public DateTime Created { get; set; }
    public bool ValidFileSystemPath() => false;
    public abstract void Delete();
    public IDirectory? Directory { get; set; }

    protected MemoryFileSystemObject(string path)
    {
      Path = path;
    }
  }
}