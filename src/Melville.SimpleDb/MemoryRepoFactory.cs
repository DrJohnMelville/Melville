using System;
using System.Data;
using System.Data.SQLite;
using System.Threading;

namespace Melville.SimpleDb;

internal sealed class MemoryRepoFactory(SQLiteConnection sqLiteConnection) :
    SqliteDiskFactory(sqLiteConnection.ConnectionString), IDisposable
{
    protected override string ReadOnlySuffix => "";

    public void Dispose()
    {
        // holding one connection makes the database continue to exist.
        sqLiteConnection.Dispose();
    }
}