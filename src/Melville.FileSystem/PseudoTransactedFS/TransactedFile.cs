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
    [FromConstructor] private readonly Lazy<IFile> innerShadow;
    [FromConstructor] public IDirectory Directory { get; private set; }

    private IFile? WeakInnerShadow => innerShadow.IsValueCreated ? innerShadow.Value : null;
    
    public string Path => inner.Path;
    public string Name => inner.Name;
    public bool Exists() => inner.Exists() | HasPendingCommit();
    public bool HasPendingCommit() => WeakInnerShadow?.Exists() ?? false;
    [DelegateTo] private IFile ActiveProxy => HasPendingCommit() ? innerShadow.Value : inner;

    public Task<Stream> OpenRead() => inner.OpenRead();

    public Task<Stream> OpenReadImmediate() =>
      inner.OpenReadImmediate();


    public void Delete()
    {
      inner.Delete();
      WeakInnerShadow?.Delete();
    }
    
    public bool ValidFileSystemPath() => inner.ValidFileSystemPath() &&
                                         (WeakInnerShadow?.ValidFileSystemPath() ?? true);

    public Task<Stream> CreateWrite(FileAttributes attributes)
    {
      Attributes = attributes;
      return innerShadow.Value.CreateWrite(attributes);
    }

    public Task Commit() => HasPendingCommit()
      ? inner.MoveFrom(innerShadow.Value, CancellationToken.None, Attributes)
      : Task.CompletedTask;

    public void Rollback() => WeakInnerShadow?.Delete();
  }