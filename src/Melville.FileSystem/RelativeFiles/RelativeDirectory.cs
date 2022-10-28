using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Melville.FileSystem.RelativeFiles;

public sealed class RelativeDirectoryRoot : RelativeDirectoryBase
{
  public IDirectory BaseDirectory { get; set; }

  public RelativeDirectoryRoot(IDirectory baseDirectory)
  {
    BaseDirectory = baseDirectory;
  }

  protected override IDirectory GetTargetDirectory() => BaseDirectory;
}
public sealed class RelativeDirectory: RelativeDirectoryBase
{
  private readonly Func<IDirectory> root;
  private readonly string name;

  public RelativeDirectory(Func<IDirectory> root, string name)
  {
    this.root = root;
    this.name = name;
  }

  protected override IDirectory GetTargetDirectory() => root().SubDirectory(name);
}

#warning -- this can become a WrappedDirectory
public abstract class RelativeDirectoryBase : DirectoryAdapterBase
{
  public override IFile File(string name) => new RelativeFile(GetTargetDirectory, name);
  public override IDirectory SubDirectory(string name) => new RelativeDirectory(GetTargetDirectory, name);
  public override IEnumerable<IFile> AllFiles() => 
    GetTargetDirectory().AllFiles().Select(MakeImmediateChild);
  public override IEnumerable<IFile> AllFiles(string glob) =>
    GetTargetDirectory().AllFiles(glob).Select(MakeImmediateChild);
  public override IEnumerable<IDirectory> AllSubDirectories() =>
    GetTargetDirectory().AllSubDirectories().Select(MakeImmediateChild);
  private IFile MakeImmediateChild(IFile arg) => File(arg.Name);
  private IDirectory MakeImmediateChild(IDirectory arg) => SubDirectory(arg.Name);
}

public abstract class DirectoryAdapterBase : IDirectory
{
  protected abstract IDirectory GetTargetDirectory();

  public virtual string Path => GetTargetDirectory().Path;
  public virtual string Name => GetTargetDirectory().Name;
  public virtual bool Exists() => GetTargetDirectory().Exists();
  public virtual IDirectory SubDirectory(string name) => GetTargetDirectory().SubDirectory(name);
  public virtual IFile File(string name) => GetTargetDirectory().File(name);
  public virtual IEnumerable<IFile> AllFiles() => GetTargetDirectory().AllFiles();
  public virtual IEnumerable<IFile> AllFiles(string glob) => GetTargetDirectory().AllFiles(glob);
  public virtual IEnumerable<IDirectory> AllSubDirectories() => GetTargetDirectory().AllSubDirectories();
  public virtual void Create(FileAttributes attributes = FileAttributes.Directory) => GetTargetDirectory().Create(attributes);
  public virtual IFile FileFromRawPath(string path) => GetTargetDirectory().FileFromRawPath(path);
  public virtual bool IsVolitleDirectory() => GetTargetDirectory().IsVolitleDirectory();
  public virtual IDisposable? WriteToken() => GetTargetDirectory().WriteToken();
  public virtual DateTime LastWrite => GetTargetDirectory().LastWrite;
  public virtual DateTime Created => GetTargetDirectory().Created;
  public virtual FileAttributes Attributes => GetTargetDirectory().Attributes;
  public bool ValidFileSystemPath() => GetTargetDirectory().ValidFileSystemPath();
  public void Delete() => GetTargetDirectory().Delete();
  public virtual DateTime LastAccess
  {
    get => GetTargetDirectory().LastAccess;
    set => GetTargetDirectory().LastAccess = value;
  }

  public virtual IDirectory? Directory => GetTargetDirectory().Directory;
}