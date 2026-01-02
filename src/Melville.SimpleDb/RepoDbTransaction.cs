using System;
using Microsoft.Data.Sqlite;

namespace Melville.SimpleDb;

public class RepoDbTransaction(SqliteTransaction transaction) :
    RepoDbConnectionImpl, IRepoDbTransaction
{
    public void Commit() => transaction.Commit();
    public void Rollback() => transaction.Rollback();

    protected override void Closing() => transaction.Dispose();

    protected override SqliteConnection GetSqliteConnection() => transaction.Connection ??
        throw new InvalidOperationException("No connection for Transaction");

}
