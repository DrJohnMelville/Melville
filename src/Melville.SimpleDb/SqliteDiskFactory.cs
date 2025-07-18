using System;
using System.Data;
using System.Data.SQLite;
using Melville.SimpleDb.LifeCycles;

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

    private SQLiteConnection CreateDbConnection(bool isReadOnly)
    {
        var ret = new SQLiteConnection(s + AddReadOnly(isReadOnly), true);
        ret.Open();
        lifecycle.ConnectionCreated(ret);
        return ret;
    }

    private string AddReadOnly(bool isReadOnly) => isReadOnly ? ReadOnlySuffix : "";

    private sealed class FileSqliteRepo(SqliteDiskFactory factory, bool isReadOnly): IRepoDbConnection
    {
        private SQLiteConnection? connection;
        /// <inheritdoc />
        public void Dispose()
        {
            if (connection is null) return;
            factory.lifecycle.ConnectionClosed(connection);
            connection.Dispose();
            connection = null;

        }

        /// <inheritdoc />
        private SQLiteConnection GetConnection() => connection ??= factory.CreateDbConnection(isReadOnly);

        IDbConnection IRepoDbConnection.GetConnection() => GetConnection();

        /// <inheritdoc />
        public SQLiteBlobWrapper BlobWrapper(string table, string column, long key, bool readOnly) => 
            new(SQLiteBlob.Create(GetConnection(), "main", table, column, key, readOnly));
    }
}