using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Melville.INPC;

namespace Melville.FileSystem.PseudoTransactedFS;

  public sealed partial class TransactedFile : IFile, ICanBypassCache
  {
    public FileAttributes Attributes { get; private set; }
    [FromConstructor] private readonly IFile inner;
    [FromConstructor] private readonly IFile innerShadow;
    [FromConstructor] public IDirectory Directory { get; private set; }
    
    public string Path => inner.Path;
    public string Name => inner.Name;
    public bool Exists() => inner.Exists() | HasPendingCommit();
    public bool HasPendingCommit() => innerShadow.Exists();
    [DelegateTo] private IFile ActiveProxy => innerShadow.Exists() ? innerShadow : inner;

    public Task<Stream> OpenRead() => inner.OpenRead();

    public Task<Stream> OpenReadImmediate() =>
      inner.OpenReadImmediate();


    public void Delete()
    {
      inner.Delete();
      innerShadow.Delete();
    }
    
    public bool ValidFileSystemPath() => inner.ValidFileSystemPath() && innerShadow.ValidFileSystemPath();

    public Task<Stream> CreateWrite(FileAttributes attributes)
    {
      Attributes = attributes;
      return innerShadow.CreateWrite(attributes);
    }

    public Task Commit() => innerShadow.Exists()
      ? inner.MoveFrom(innerShadow, CancellationToken.None, Attributes)
      : Task.CompletedTask;

    public void Rollback() => innerShadow.Delete();
  }