using Dapper;
using Melville.SimpleDb;
using Melville.SimpleDb.LifeCycles;

namespace Melville.FileSystem.Sqlite;

public class FileSystemDatabaseLifecycle(Migration[] migrations): MigratedWalLifecycle(migrations)
{
    public override void DatabaseOpened(IRepoDbConnection connection)
    {
        connection.GetConnection().Execute("PRAGMA locking_mode = EXCLUSIVE;");
        base.DatabaseOpened(connection);
    }
}