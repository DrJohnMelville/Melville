using Dapper;
using System.Data;
using System.Data.Common;

namespace Melville.SimpleDb.LifeCycles;

public interface IDatabaseLifecycle
{
    /// <summary>
    /// Prepare a new database upon opening.  The caller will dispose the connection
    /// </summary>
    /// <param name="connection">A connection to the new database</param>
    public void DatabaseOpened(IRepoDbConnection connection);

    /// <summary>
    /// Close a database connection.  The caller will dispose the connection.
    /// </summary>
    /// <param name="connection">A connection to the database being closed.</param>
    public void DatabaseClosed(IRepoDbConnection connection);

    /// <summary>
    /// Initialize a newly created connection.  The caller will dispose the connection.
    /// </summary>
    /// <param name="connection">The connection to initialize.</param>
    public void ConnectionCreated(IDbConnection connection);

    /// <summary>
    /// Finalize a connection that is being closed.  The caller will dispose the connection.
    /// </summary>
    /// <param name="connection">The connection being disposed o.</param>
    public void ConnectionClosed(IDbConnection connection);
}

public class MigratedDatabaseLifecycle(Migration[] migrations) : IDatabaseLifecycle
{
    public virtual void DatabaseOpened(IRepoDbConnection connection)
    {
        new Migrator(migrations).UpgradeToCurrentSchema(connection.GetConnection());
    }
    public virtual void DatabaseClosed(IRepoDbConnection connection)
    {
    }
    public virtual void ConnectionCreated(IDbConnection connection)
    {
        connection.Execute("PRAGMA foreign_keys = ON");
    }
    public virtual void ConnectionClosed(IDbConnection connection)
    {
        connection.Execute("PRAGMA optimize;");
    }
}

public class MigratedWalLifecycle(Migration[] migrations) : MigratedDatabaseLifecycle(migrations)
{
    public override void ConnectionCreated(IDbConnection connection)
    {
        base.ConnectionCreated(connection);
        connection.Execute("PRAGMA journal_mode = 'wal';"); // Enable WAL mode for better concurrency 
    }
}