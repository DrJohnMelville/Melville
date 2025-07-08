using System;
using System.Data;
using System.Linq;
using Dapper;
using Melville.INPC;

namespace Melville.SimpleDb;

[Obsolete("Use MigratedRepoConnection instead.")]
public class MigratedRepo(IRepoDbConnection repo, Migrator migator, int desiredVersion = Int32.MaxValue) : IRepoDbConnection
{
    public IDbConnection GetConnection()
    {
        var connection = repo.GetConnection();
        migator.UpgradeToVersion(connection, desiredVersion);
        return connection;
    }

    /// <inheritdoc />
    public IRepoDbConnection Clone() => this;

    /// <inheritdoc />
    public SQLiteBlobWrapper BlobWrapper(string table, string column, long key, bool readOnly) => 
        repo.BlobWrapper(table, column, key, readOnly);
}

public partial class Migrator
{
    [FromConstructor] private readonly Migration[] migrations;

    partial void OnConstructed()
    {
        Array.Sort(migrations);
    }

    public void UpgradeToCurrentSchema(IDbConnection connection) =>
        UpgradeToVersion(connection, CurrentSchemaVersion());

    private int CurrentSchemaVersion() => migrations.Max(i => i.Version);

    public void UpgradeToVersion(IDbConnection connection, int desiredVersion)
    {
        var oldVersion = QueryCurrentVersion(connection);
        if (desiredVersion <= oldVersion) return;
        
        using var transaction = connection.BeginTransaction();
        
        ExecuteUpdateScripts(connection, transaction, oldVersion, desiredVersion);
        StoreFinalVersion(connection, transaction, desiredVersion);
        transaction.Commit();
    }

    private void ExecuteUpdateScripts(IDbConnection connection, IDbTransaction transaction, int oldVersion,
        int finalVersion)
    {
        foreach (var migration in migrations)
        {
            if (migration.Version > oldVersion && migration.Version <= finalVersion)
            {
                connection.Execute(migration.UpgradeScript, transaction);
            }
        }
    }

    private static int QueryCurrentVersion(IDbConnection connection) =>
        connection.QuerySingle<int>("PRAGMA user_version");

    private static void StoreFinalVersion(
        IDbConnection connection, IDbTransaction transaction, int newVersion) =>
        connection.Execute($"PRAGMA user_version={newVersion}", transaction);
}