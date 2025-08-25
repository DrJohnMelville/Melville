using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Melville.Hacks.Reflection;
using SQLitePCL;

namespace Melville.SimpleDb;

public interface IRepoDbConnection: IDisposable
{
    IDbConnection GetConnection();

    Stream BlobWrapper(string table, string column, long key, bool readOnly);
}
