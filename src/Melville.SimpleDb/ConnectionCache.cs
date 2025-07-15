using System.Data;
using System.Data.SQLite;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Melville.SimpleDb;

public class RepoDbConfiguration
{
    public string FolderPath { get; set; } = "";

    private static volatile int uniqueNum=1;
    public string ConnectionString => IsOnDiskDatabase() ? 
        $"DataSource={FolderPath}" : $"FullUri=file:mem{Interlocked.Increment(ref uniqueNum)}.db?mode=memory&cache=shared";

    public bool IsOnDiskDatabase() => FolderPath.Length > 0;

    public bool TestData { get; set; }

    public IRepoConnectionFactory CreateFactory(Migration[] migrations)
    {
        var ret = CreateFactory();
        using var conn = ret.Create();
        var dbConnection = conn.GetConnection();
        ConfigureDatabase(dbConnection);
        new Migrator(migrations).UpgradeToCurrentSchema(dbConnection);
        return ret;
    }

    private static void ConfigureDatabase(IDbConnection dbConnection)
    {
        dbConnection.Execute("PRAGMA foreign_keys = ON");
        dbConnection.Execute("PRAGMA journal_mode = 'wal';"); // Enable WAL mode for better concurrency 
        dbConnection.Execute("PRAGMA locking_mode = EXCLUSIVE;");
    }

    private IRepoConnectionFactory CreateFactory() =>
        IsOnDiskDatabase() ?
            new SqliteDiskFactory(ConnectionString) :
            new MemoryRepoFactory(new SQLiteConnection(ConnectionString).OpenAndReturn());
}

public interface IRepoConnectionFactory
{
    IRepoDbConnection Create();
    IRepoDbConnection CreateReadOnly();
}

