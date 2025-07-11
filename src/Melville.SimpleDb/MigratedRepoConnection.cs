using System;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace Melville.SimpleDb;

[Obsolete("Such a short life -- never made it to a full release")]
public class MigratedRepoConnection: IRepoDbConnection, IDisposable
{
    static MigratedRepoConnection()
    {
        SqlMapper.RemoveTypeMap(typeof(DateTimeOffset));
        SqlMapper.AddTypeHandler(new DateTimeOffsetTypeHandler());

    }
    private readonly SQLiteConnection connection;
    public MigratedRepoConnection(RepoDbConfiguration config, Migration[] schema)
    {
        connection = CreateConnection(config);
        new Migrator(schema).UpgradeToCurrentSchema(connection);
    }

    private MigratedRepoConnection(SQLiteConnection connection) => 
        this.connection = connection;

    private SQLiteConnection CreateConnection(RepoDbConfiguration config)
    {
        var ret = new SQLiteConnection(config.ConnectionString, true);
        ret.Open();
        ret.Execute("PRAGMA foreign_keys = ON");
        ret.Execute("PRAGMA journal_mode = 'wal';"); // Enable WAL mode for better concurrency 

        return ret;
    }
    public IDbConnection GetConnection() => connection;

    /// <inheritdoc />
    public IRepoDbConnection Clone()
    {
        if (connection.ConnectionString.Contains(":memory:"))
            return this;
        return new MigratedRepoConnection(
            new SQLiteConnection(connection.ConnectionString, true)
                .OpenAndReturn());
    }

    /// <inheritdoc />
    public SQLiteBlobWrapper BlobWrapper(string table, string column, long key, bool readOnly)
    {
        return new(SQLiteBlob.Create(connection,
            "main", table, column, key, readOnly));
    }

    /// <inheritdoc />
    public void Dispose() => connection.Dispose();
}