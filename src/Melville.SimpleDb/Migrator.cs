using System;
using System.Data;
using System.Linq;
using Dapper;
using Melville.INPC;

namespace Melville.SimpleDb;

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
        var oldVersion = connection.Query<int>("PRAGMA user_version").First();
        if (desiredVersion <= oldVersion) return;
        TryDatabaseUpgrade(connection, oldVersion, desiredVersion);
    }

    private void TryDatabaseUpgrade(IDbConnection connection, int oldVersion, int finalVersion)
    {
        using var transaction = connection.BeginTransaction();
        ExecuteDatabaseUpgrade(connection, oldVersion, finalVersion);
        transaction.Commit();
    }

    private void ExecuteDatabaseUpgrade(IDbConnection connection, int oldVersion, int finalVersion)
    {
        ExecuteUpdateScripts(connection, oldVersion, finalVersion);
        StoreFinalVersion(connection, finalVersion);
    }

    private void ExecuteUpdateScripts(IDbConnection connection, int oldVersion, int finalVersion)
    {
        foreach (var migration in migrations)
        {
            if (migration.Version > oldVersion && migration.Version <= finalVersion)
            {
                connection.Execute(migration.UpgradeScript);
            }
        }
    }

    private static void StoreFinalVersion(IDbConnection connection, int newVersion) =>
        connection.Execute($"PRAGMA user_version={newVersion}");
}