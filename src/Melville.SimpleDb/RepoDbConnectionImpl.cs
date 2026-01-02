using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Melville.SimpleDb;

public abstract class RepoDbConnectionImpl() : IRepoDbConnection
{
    private SqliteConnection? connection = null;
    public IRepoDbTransaction BeginTransaction() =>
        new RepoDbTransaction(GetOrCreateConnection().BeginTransaction());

    public Stream BlobWrapper(string table, string column, long key, bool readOnly)=> 
        new SqliteBlob(GetOrCreateConnection(), table, column, key, readOnly);

    public void Dispose()
    {
        if (connection is not { State: ConnectionState.Open }) return;
        Closing();
        connection.Dispose();
        connection = null;
    }

    public IDbConnection GetConnection() => GetOrCreateConnection();

    private SqliteConnection GetOrCreateConnection() => 
        connection ??= GetSqliteConnection();
    protected abstract SqliteConnection GetSqliteConnection();
    protected virtual void Closing() { }
}