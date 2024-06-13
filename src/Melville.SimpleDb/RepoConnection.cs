using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;

namespace Melville.SimpleDb;

public class RepoConnection(RepoDbConfiguration config) : IRepoDbConnection
{
    static RepoConnection()
    {
        SqlMapper.RemoveTypeMap(typeof(DateTimeOffset));
        SqlMapper.AddTypeHandler(new DateTimeOffsetTypeHandler());
    }
    public ValueTask<IDbConnection> GetConnectionAsync()
    {
        var ret = new SQLiteConnection(config.ConnectionString);
        ret.Open();
        ret.Execute("PRAGMA foreign_keys = ON");
        return new(ret);
    }
}