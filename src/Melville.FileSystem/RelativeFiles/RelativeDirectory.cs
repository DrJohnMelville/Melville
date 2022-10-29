using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melville.INPC;

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

public abstract  partial class RelativeDirectoryBase : IDirectory
{
  [DelegateTo]protected abstract IDirectory GetTargetDirectory();
  public IFile File(string name) => new RelativeFile(GetTargetDirectory, name);
  public IDirectory SubDirectory(string name) => new RelativeDirectory(GetTargetDirectory, name);
  public IEnumerable<IFile> AllFiles() => 
    GetTargetDirectory().AllFiles().Select(MakeImmediateChild);
  public IEnumerable<IFile> AllFiles(string glob) =>
    GetTargetDirectory().AllFiles(glob).Select(MakeImmediateChild);
  public IEnumerable<IDirectory> AllSubDirectories() =>
    GetTargetDirectory().AllSubDirectories().Select(MakeImmediateChild);
  
  private IFile MakeImmediateChild(IFile arg) => File(arg.Name);
  private IDirectory MakeImmediateChild(IDirectory arg) => SubDirectory(arg.Name);
}