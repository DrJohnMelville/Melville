using System;
using System.Data;
using System.Data.SQLite;

namespace Melville.SimpleDb;

internal class SqliteDiskFactory(string connectionString) : IRepoConnectionFactory
{
    /// <inheritdoc />
    public IRepoDbConnection Create() => 
        new FileSqliteRepo(CreateDbConnection(connectionString));

    protected virtual string ReadOnlySuffix => ";Read Only=True";


    /// <inheritdoc />
    public IRepoDbConnection CreateReadOnly() => 
        new FileSqliteRepo(CreateDbConnection(connectionString + ReadOnlySuffix));

    private SQLiteConnection CreateDbConnection(string connStr) =>
        new SQLiteConnection(connStr, true).OpenAndReturn();

    private sealed class FileSqliteRepo(SQLiteConnection connection): IRepoDbConnection
    {
        private SQLiteConnection? connection = connection;
        /// <inheritdoc />
        public void Dispose()
        {
            connection.Dispose();
            connection = null;

        }

        /// <inheritdoc />
        public SQLiteConnection GetConnection()
        {
            return connection ??
                   throw new ObjectDisposedException(nameof(FileSqliteRepo));
        }
        IDbConnection IRepoDbConnection.GetConnection() => GetConnection();

        /// <inheritdoc />
        public SQLiteBlobWrapper BlobWrapper(string table, string column, long key, bool readOnly) => 
            new(SQLiteBlob.Create(GetConnection(), "main", table, column, key, readOnly));
    }
}