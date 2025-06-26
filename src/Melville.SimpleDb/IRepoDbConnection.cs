using System;
using System.Data;
using System.Threading.Tasks;

namespace Melville.SimpleDb;

public interface IRepoDbConnection
{
    [Obsolete("Sqlite is synchronous so use GetConnection instead")]
    ValueTask<IDbConnection> GetConnectionAsync() => ValueTask.FromResult(GetConnection());
    IDbConnection GetConnection();
}