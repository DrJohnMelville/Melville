using System.Data;
using System.Data.SQLite;
using System.Threading;
using Dapper;
using Melville.SimpleDb.LifeCycles;

namespace Melville.SimpleDb;


public class RepoDbConfiguration
{
    public string FolderPath { get; set; } = "";

    private static volatile int uniqueNum=1;
    public string ConnectionString => IsOnDiskDatabase() ? 
        $"DataSource={FolderPath}" : 
        $"FullUri=file:mem{Interlocked.Increment(ref uniqueNum)}.db?mode=memory&cache=shared";

    public bool IsOnDiskDatabase() => FolderPath.Length > 0;

    public bool TestData { get; set; }

    public IRepoConnectionFactory CreateFactory(IDatabaseLifecycle lifecycle)
    {
        return IsOnDiskDatabase() ?
            new SqliteDiskFactory(ConnectionString, lifecycle) :
            new MemoryRepoFactory(new SQLiteConnection(ConnectionString).OpenAndReturn(),
                lifecycle);
    }
}