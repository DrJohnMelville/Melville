using System;
using System.IO;
using System.Threading.Tasks;

namespace Melville.FileSystem.FileSystem.RelativeFiles
{

  public sealed class RelativeFile: FileAdapeterBase
  {
    private readonly Func<IDirectory> concreteParent;
    private readonly string fileName;

    public RelativeFile(Func<IDirectory> concreteParent, string fileName)
    {
      this.concreteParent = concreteParent;
      this.fileName = fileName;
    }
    protected override IFile InnerFile() => concreteParent().File(fileName);
  }
  public abstract class FileAdapeterBase:IFile
  {
    protected abstract IFile InnerFile();

    #region MyRegion
    public string Path => InnerFile().Path;
    public string Name => InnerFile().Name;
    public bool Exists() => InnerFile().Exists();
    public Task<Stream> OpenRead() => InnerFile().OpenRead();
    public Task<Stream> CreateWrite(FileAttributes attributes = FileAttributes.Normal) => 
      InnerFile().CreateWrite(attributes);
    public void Delete() => InnerFile().Delete();
    public byte FinalProgress => InnerFile().FinalProgress;
    public Task WaitForFinal => InnerFile().WaitForFinal;
    public bool ValidFileSystemPath() => InnerFile().ValidFileSystemPath();
    public IDirectory? Directory => InnerFile().Directory;
    public long Size => InnerFile().Size;
    public DateTime LastAccess
    {
      get => InnerFile().LastAccess;
      set => InnerFile().LastAccess = value;
    }
    public DateTime LastWrite => InnerFile().LastWrite;
    public DateTime Created => InnerFile().Created;
    public FileAttributes Attributes => InnerFile().Attributes;

    #endregion
  }
}