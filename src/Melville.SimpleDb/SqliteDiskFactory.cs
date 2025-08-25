using System;
using System.Data;
using System.IO;
using Melville.SimpleDb.LifeCycles;
using Microsoft.Data.Sqlite;

namespace Melville.SimpleDb;

internal class SqliteDiskFactory : IRepoConnectionFactory
{
    private readonly string s;
    private readonly IDatabaseLifecycle lifecycle;

    public SqliteDiskFactory(string connectionString, IDatabaseLifecycle lifecycle)
    {
        s = connectionString;
        this.lifecycle = lifecycle;
        using var conn = Create();
        lifecycle.DatabaseOpened(conn);
    }

    public virtual void Dispose()
    {
        using var conn = Create();
        lifecycle.DatabaseClosed(conn);
    }

    /// <inheritdoc />
    public IRepoDbConnection Create() => 
        new FileSqliteRepo(this, false);

    protected virtual string ReadOnlySuffix => ";Read Only=True";


    /// <inheritdoc />
    public IRepoDbConnection CreateReadOnly() => 
        new FileSqliteRepo(this, true);

    private SqliteConnection CreateDbConnection(bool isReadOnly)
    {
        var ret = new SqliteConnection(s + AddReadOnly(isReadOnly));
        ret.Open();
        lifecycle.ConnectionCreated(ret);
        return ret;
    }

    private string AddReadOnly(bool isReadOnly) => isReadOnly ? ReadOnlySuffix : "";

    private sealed class FileSqliteRepo(SqliteDiskFactory factory, bool isReadOnly): IRepoDbConnection
    {
        private SqliteConnection? connection;
        /// <inheritdoc />
        public void Dispose()
        {
            if (connection is not {State: ConnectionState.Open}) return;
            factory.lifecycle.ConnectionClosed(connection);
            connection.Dispose();
            connection = null;

        }

        /// <inheritdoc />
        private SqliteConnection GetConnection() => connection ??= factory.CreateDbConnection(isReadOnly);

        IDbConnection IRepoDbConnection.GetConnection() => GetConnection();

        /// <inheritdoc />
        public Stream BlobWrapper(string table, string column, long key, bool readOnly) => 
            new SqliteBlob(GetConnection(), table, column, key, readOnly);
    }
}