using System;
using System.Threading.Tasks;
using Melville.FileSystem.PseudoTransactedFS;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public class BlockTransactableStore(BlockRootDirectory dir): ITransactableStore
{
    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        dir.Dispose();
        return new ValueTask();
    }

    /// <inheritdoc />
    public IDisposable? WriteToken() => new FakeToken();

    private class FakeToken : IDisposable
    {
        public void Dispose()
        {
            // Do nothing
        }
    }

    /// <inheritdoc />
    public IDirectory UntransactedRoot => dir;

    /// <inheritdoc />
    public ITransactedDirectory BeginTransaction() => new BlockTransaction(dir);

    /// <inheritdoc />
    public bool IsLocalStore => true;

    /// <inheritdoc />
    public IDownloadProgressStore? ProgressStore => null;

    /// <inheritdoc />
    public Task<bool> RenewLease() => Task.FromResult(true);
}