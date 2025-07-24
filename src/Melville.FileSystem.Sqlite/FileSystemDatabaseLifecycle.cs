using Dapper;
using Melville.SimpleDb;
using Melville.SimpleDb.LifeCycles;

namespace Melville.FileSystem.Sqlite;

public class FileSystemDatabaseLifecycle(Migration[] migrations): MigratedWalLifecycle(migrations)
{
    public override void DatabaseOpened(IRepoDbConnection connection)
    {
        base.DatabaseOpened(connection);
        connection.GetConnection().Execute("PRAGMA locking_mode = EXCLUSIVE;");
    }

    /// <inheritdoc />
    public override void DatabaseClosed(IRepoDbConnection connection)
    {
        // open and close to delete wal file.
        base.DatabaseClosed(connection);
    }
}