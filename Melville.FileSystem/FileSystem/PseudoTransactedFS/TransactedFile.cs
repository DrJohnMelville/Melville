using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.FileSystem.PseudoTransactedFS
{
  public partial class TransactedDirectory
  {
    public sealed partial class TransactedFile : IFile, ICanBypassCache
    {
      public FileAttributes Attributes { get; private set; }
      private readonly IFile inner;
      private readonly IFile innerShadow; // shadow is the write beside file before the transaction commits;
      public IDirectory Directory { get; private set; }
      private TransactedFile(IFile inner, IFile innerShadow, IDirectory directory)
      {
        this.inner = inner;
        this.innerShadow = innerShadow;
        Directory = directory;
      }
      public string Path => inner.Path;
      public string Name => inner.Name;
      public bool Exists() => inner.Exists() | innerShadow.Exists();
      private IFile ActiveProxy => innerShadow.Exists() ? innerShadow : inner;
      public long Size => ActiveProxy.Size;

      public DateTime LastAccess
      {
        get { return ActiveProxy.LastAccess; }
        set { ActiveProxy.LastAccess = value; }
      }
      public DateTime LastWrite => ActiveProxy.LastWrite;
      public DateTime Created => ActiveProxy.Created;
      public Task<Stream> OpenRead() => inner.OpenRead();

      public Task<Stream> OpenReadImmediate() =>
        inner.OpenReadImmediate();


      public void Delete()
      {
        inner.Delete();
        innerShadow.Delete();
      }
      public bool ValidFileSystemPath() => inner.ValidFileSystemPath() && innerShadow.ValidFileSystemPath();

      #region Write and Commit
      public Task<Stream> CreateWrite(FileAttributes attributes)
      {
        this.Attributes = attributes;
        return (inner.Exists()?innerShadow:inner).CreateWrite(attributes);
      }

      public Task Commit() => innerShadow.Exists()
        ? inner.MoveFrom(innerShadow, CancellationToken.None, Attributes)
        : Task.CompletedTask;
      #endregion
    }
  }
}