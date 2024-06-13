using System;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace Melville.SimpleDb;

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
        return ret;
    }
}