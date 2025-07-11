using System.Data;
using System.Data.SQLite;
using Dapper;
using Melville.SimpleDb;

namespace Melville.FileSystem.Sqlite;

public readonly struct SqliteTransactionScope
{
    private readonly IRepoDbConnection connection;
    private readonly IDbTransaction? transaction;

    public SqliteTransactionScope(IRepoDbConnection connection, bool withTransaction)
    {
        this.connection = connection;
        if (withTransaction)
        {
            transaction = connection.GetConnection().BeginTransaction();
        }
    }

    IDbConnection Target() => connection.GetConnection();

    public void Execute(string sql, object? param = null) =>
        Target().Execute(sql, param, transaction);
    public Task ExecuteAsync(string sql, object? param = null) =>
        Target().ExecuteAsync(sql, param, transaction);
    public T? ExecuteScalar<T>(string sql, object? param = null) =>
        Target().ExecuteScalar<T>(sql, param, transaction);
    public Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null) =>
        Target().ExecuteScalarAsync<T>(sql, param, transaction);

    public T? QueryFirstOrDefault<T>(string sql, object? param = null) =>
        Target().QueryFirstOrDefault<T>(sql, param, transaction);

    public T? QuerySingleOrDefault<T>(string sql, object? param = null) =>
        Target().QuerySingleOrDefault<T>(sql, param, transaction);

    public IEnumerable<T> Query<T>(string sql, object? param = null) =>
        Target().Query<T>(sql, param, transaction);

    public SQLiteBlobWrapper BlobWrapper(long blockId, bool readOnly) =>
        connection.BlobWrapper("Blocks", "Bytes", blockId, readOnly);

    public void Commit() => transaction?.Commit();
    public void Rollback() => transaction?.Rollback();

    public void Dispose()
    {
        transaction?.Dispose();
        connection.Dispose();
    }
}