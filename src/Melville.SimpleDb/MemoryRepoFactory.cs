using Melville.SimpleDb.LifeCycles;
using Microsoft.Data.Sqlite;

namespace Melville.SimpleDb;

internal sealed class MemoryRepoFactory(
    SqliteConnection sqLiteConnection, IDatabaseLifecycle lifecycle) :
    SqliteDiskFactory(sqLiteConnection.ConnectionString, lifecycle)
{
    protected override string ReadOnlySuffix => "";

    public override void Dispose()
    {
        base.Dispose();
        // holding one connection makes the database continue to exist.
        sqLiteConnection.Dispose();
    }
}