using System;
using Melville.INPC;

namespace Melville.FileSystem.RelativeFiles;

public sealed partial class RelativeFile: IFile
{
  private readonly Func<IDirectory> concreteParent;
  private readonly string fileName;

  public RelativeFile(Func<IDirectory> concreteParent, string fileName)
  {
    this.concreteParent = concreteParent;
    this.fileName = fileName;
  }
  [DelegateTo] private IFile InnerFile() => concreteParent().File(fileName);
}