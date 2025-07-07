using System;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace Melville.SimpleDb;

[Obsolete("Use MigratedRepoConnection instead.")]
public class RepoConnection(RepoDbConfiguration config) : IRepoDbConnection
{
    static RepoConnection()
    {
        SqlMapper.RemoveTypeMap(typeof(DateTimeOffset));
        SqlMapper.AddTypeHandler(new DateTimeOffsetTypeHandler());
    }
    public IDbConnection GetConnection()
    {
        var ret = new SQLiteConnection(config.ConnectionString);
        ret.Open();
        ret.Execute("PRAGMA foreign_keys = ON");
        ret.Execute("PRAGMA journal_mode = 'wal';"); // Enable WAL mode for better concurrency 

        return ret;
    }

    /// <inheritdoc />
    public IRepoDbConnection Clone() => this;

    /// <inheritdoc />
    public SQLiteBlobWrapper BlobWrapper(string table, string column, long key, bool readOnly) => 
        throw new NotImplementedException();
}