using System.Data;
using System.Threading;
using Dapper;
using Melville.SimpleDb.LifeCycles;
using Microsoft.Data.Sqlite;

namespace Melville.SimpleDb;


public class RepoDbConfiguration
{
    public string FolderPath { get; set; } = "";
    public string Password { get; set; } = "";

    private static volatile int uniqueNum=1;
    public string ConnectionString => IsOnDiskDatabase() ? 
        $"DataSource={FolderPath}" + TryAddPassword() : 
        $"Data Source=file:mem{Interlocked.Increment(ref uniqueNum)}.db?mode=memory&cache=shared";

    private string TryAddPassword() => Password.Length > 0 ? $";Password={Password}" : "";

    public bool IsOnDiskDatabase() => FolderPath.Length > 0;

    public bool TestData { get; set; }

    public IRepoConnectionFactory CreateFactory(IDatabaseLifecycle lifecycle)
    {
        return IsOnDiskDatabase() ?
            new SqliteDiskFactory(ConnectionString, lifecycle) :
            new MemoryRepoFactory(new SqliteConnection(ConnectionString).OpenAndReturn(),
                lifecycle);
    }
}

public static class SqliteExtensions
{
    public static SqliteConnection OpenAndReturn(this SqliteConnection connection)
    {
        connection.Open();
        return connection;
    }
}