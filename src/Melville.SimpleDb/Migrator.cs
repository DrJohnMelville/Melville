using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Melville.INPC;

namespace Melville.SimpleDb;

public class MigratedRepo(IRepoDbConnection repo, Migrator migator, int desiredVersion = Int32.MaxValue) : IRepoDbConnection
{
    public async ValueTask<IDbConnection> GetConnectionAsync()
    {
        var connection = await repo.GetConnectionAsync();
        await migator.UpgradeToVersionAsync(connection, desiredVersion);
        return connection;
    }
}

public partial class Migrator
{
    [FromConstructor] private readonly Migration[] migrations;

    partial void OnConstructed()
    {
        Array.Sort(migrations);
    }

    public ValueTask UpgradeToCurrentSchemaAsync(IDbConnection connection) =>
        UpgradeToVersionAsync(connection, CurrentSchemaVersion());

    private int CurrentSchemaVersion() => migrations.Max(i => i.Version);

    public async ValueTask UpgradeToVersionAsync(IDbConnection connection, int desiredVersion)
    {
        var oldVersion = await QueryCurrentVersion(connection);
        if (desiredVersion <= oldVersion) return;
        
        using var transaction = connection.BeginTransaction();
        
        await ExecuteUpdateScriptsAsync(connection, oldVersion, desiredVersion);
        await StoreFinalVersion(connection, desiredVersion);
        transaction.Commit();
    }

    private async ValueTask ExecuteUpdateScriptsAsync(IDbConnection connection, int oldVersion, int finalVersion)
    {
        foreach (var migration in migrations)
        {
            if (migration.Version > oldVersion && migration.Version <= finalVersion)
            {
                await connection.ExecuteAsync(migration.UpgradeScript);
            }
        }
    }

    private static Task<int> QueryCurrentVersion(IDbConnection connection) =>
        connection.QuerySingleAsync<int>("PRAGMA user_version");

    private static Task StoreFinalVersion(IDbConnection connection, int newVersion) =>
        connection.ExecuteAsync($"PRAGMA user_version={newVersion}");
}