using System.Data;
using System.Data.SQLite;
using Dapper;

namespace Melville.FileSystem.Sqlite;

public readonly struct SqliteTransactionScope(
    IDbConnection connection,
    IDbTransaction? transaction)
{
    public void Execute(string sql, object? param = null) =>
        connection.Execute(sql, param, transaction);
    public Task ExecuteAsync(string sql, object? param = null) =>
        connection.ExecuteAsync(sql, param, transaction);
    public T? ExecuteScalar<T>(string sql, object? param = null) =>
        connection.ExecuteScalar<T>(sql, param, transaction);
    public Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null) =>
        connection.ExecuteScalarAsync<T>(sql, param, transaction);

    public T? QueryFirstOrDefault<T>(string sql, object? param = null) =>
        connection.QueryFirstOrDefault<T>(sql, param, transaction);

    public T? QuerySingleOrDefault<T>(string sql, object? param = null) =>
        connection.QuerySingleOrDefault<T>(sql, param, transaction);

    public IEnumerable<T> Query<T>(string sql, object? param = null) =>
        connection.Query<T>(sql, param, transaction);

    public SQLiteBlobWrapper BlobWrapper(long blockId, bool readOnly) =>
        new(SQLiteBlob.Create((SQLiteConnection) connection,
                          "main", "Blocks", "Bytes", blockId, readOnly));

    public SqliteTransactionScope NewTransactions() =>
        new(connection, connection.BeginTransaction());

    public void Commit() => transaction?.Commit();
    public void Rollback() => transaction?.Rollback();
    public void DisposeTransaction() =>  transaction?.Dispose();

    public void Dispose()
    {
        DisposeTransaction();
        connection.Dispose();
    }
}