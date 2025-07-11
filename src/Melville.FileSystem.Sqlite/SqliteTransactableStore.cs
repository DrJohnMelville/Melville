using Melville.FileSystem.PseudoTransactedFS;
using Melville.SimpleDb;

namespace Melville.FileSystem.Sqlite;

public class SqliteTransactableStore(IRepoConnectionFactory repo) : ITransactableStore
{
    private readonly SqliteTransactionScope readerScope = 
        new SqliteTransactionScope(repo.CreateReadOnly(), false);

    public SqliteTransactableStore(string path): this(new RepoDbConfiguration()
    {
        FolderPath = path
    }.CreateFactory(DbTables.All))
    {
    }
    
    public ValueTask DisposeAsync()
    {
        readerScope.Dispose();
        return ValueTask.CompletedTask;
    }

    public IDisposable? WriteToken() => new FakeDisposable();

    private class FakeDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }

    public IDirectory UntransactedRoot => new SqliteFileStore(readerScope).GetRoot("");
    public ITransactedDirectory BeginTransaction() => 
       new SqliteFileStore(new SqliteTransactionScope(repo.Create(), true))
           .GetTransactedRoot("");   

    public bool IsLocalStore => true;
    public IDownloadProgressStore? ProgressStore => null;
    public Task<bool> RenewLease() => Task.FromResult(true);
}