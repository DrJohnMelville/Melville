using System.Data;

namespace Melville.SimpleDb;

public class MigratedRepoConnection: IRepoDbConnection
{
    private IDbConnection connection;
    public MigratedRepoConnection(RepoDbConfiguration config, Migration[] schema)
    {
        connection = new RepoConnection(config).GetConnection();
        new Migrator(schema).UpgradeToCurrentSchema(connection);
    }

    public IDbConnection GetConnection() => connection;
}