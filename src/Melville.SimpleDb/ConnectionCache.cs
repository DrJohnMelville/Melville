using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.SimpleDb;


public class ConnectionCache(IRepoDbConnection inner) : IRepoDbConnection
{
    private readonly Lazy<Task<IDbConnection>> connection =
        new(() => inner.GetConnectionAsync().AsTask(), LazyThreadSafetyMode.ExecutionAndPublication);

    public ValueTask<IDbConnection> GetConnectionAsync() => new(connection.Value);
}

public class RepoDbConfiguration
    {
        public string FolderPath { get; set; } = "";

        public string ConnectionString => IsOnDiskDatabase() ?
            $"DataSource={FolderPath}" :
            "DataSource=:memory:";

        public bool IsOnDiskDatabase() => FolderPath.Length > 0;

        public bool TestData { get; set; }
    }