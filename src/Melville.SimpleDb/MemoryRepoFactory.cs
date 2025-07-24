using System.Data.SQLite;
using Melville.SimpleDb.LifeCycles;

namespace Melville.SimpleDb;

internal sealed class MemoryRepoFactory(
    SQLiteConnection sqLiteConnection, IDatabaseLifecycle lifecycle) :
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