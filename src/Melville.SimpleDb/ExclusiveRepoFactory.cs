using System;
using System.Data;
using System.Data.SQLite;
using System.Threading;

namespace Melville.SimpleDb;

internal sealed class ExclusiveRepoFactory(SQLiteConnection sqLiteConnection) : IRepoConnectionFactory
{
    private readonly SQLiteConnection sqLiteConnection = sqLiteConnection;
    private readonly SemaphoreSlim mutex = new(1, 1);

    /// <inheritdoc />
    public IRepoDbConnection Create()
    {
        mutex.Wait();
        return new ExclusiveRepoDbFactory(this);
    }

    public IRepoDbConnection CreateReadOnly() => new SharedDbConnection(sqLiteConnection);


    internal sealed class ExclusiveRepoDbFactory(ExclusiveRepoFactory parent) : IRepoDbConnection
    {
        private ExclusiveRepoFactory? parent = parent;

        /// <inheritdoc />
        public void Dispose()
        {
            parent?.mutex.Release();
            parent = null;
        }

        /// <inheritdoc />
        public SQLiteConnection GetConnection()
        {
            return (parent?.sqLiteConnection ??
                    throw new ObjectDisposedException(nameof(ExclusiveRepoDbFactory)));
        }

        IDbConnection IRepoDbConnection.GetConnection() => GetConnection();

        /// <inheritdoc />
        public SQLiteBlobWrapper BlobWrapper(string table, string column, long key, bool readOnly) =>
            new(SQLiteBlob.Create(GetConnection(), "main", table, column, key, readOnly));
    }


    private class SharedDbConnection(SQLiteConnection sqLiteConnection) : IRepoDbConnection
    {
        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public IDbConnection GetConnection() => sqLiteConnection;

        /// <inheritdoc />
        public SQLiteBlobWrapper BlobWrapper(string table, string column, long key, bool readOnly) =>
            new(
                SQLiteBlob.Create(sqLiteConnection, "main", table, column, key, readOnly));
    }
}