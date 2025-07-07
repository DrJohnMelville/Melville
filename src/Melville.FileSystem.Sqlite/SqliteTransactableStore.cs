using Melville.FileSystem.PseudoTransactedFS;

namespace Melville.FileSystem.Sqlite;

public class SqliteTransactableStore(SqliteFileStore fileStore) : ITransactableStore
{
    public SqliteTransactableStore(string path): this(SqliteFileStore.Create(path))
    {
    }
    
    public ValueTask DisposeAsync()
    {
        fileStore.Dispose();
        return ValueTask.CompletedTask;
    }

    public IDisposable? WriteToken() => new FakeDisposable();

    private class FakeDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }

    public IDirectory UntransactedRoot => fileStore.UntransactedRoot("");
    public ITransactedDirectory BeginTransaction() => fileStore.TransactedRoot("");   

    public bool IsLocalStore => true;
    public IDownloadProgressStore? ProgressStore => null;
    public Task<bool> RenewLease() => Task.FromResult(true);
}