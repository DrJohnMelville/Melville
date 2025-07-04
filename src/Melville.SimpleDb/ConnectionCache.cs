﻿using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.SimpleDb;

[Obsolete("Use MigratedRepoConnection instead.")]
public class ConnectionCache(IRepoDbConnection inner) : IRepoDbConnection
{
    private readonly Lazy<IDbConnection> connection =
        new(inner.GetConnection, LazyThreadSafetyMode.ExecutionAndPublication);

    public IDbConnection GetConnection() => connection.Value;
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