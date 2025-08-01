﻿using System.Linq;
using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.FileSystem.PseudoTransactedFS;
using Melville.FileSystem.Sqlite;
using Melville.Mvvm.TestHelpers.MockFiles;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.PseudoTransactedFS;

public class TransactedDirectoryTest : RootTransactedDirectoryTest
{
    public TransactedDirectoryTest() : this(new MockDirectory("j:\\dir"))
    {
    }

    private TransactedDirectoryTest(MockDirectory dir): base(dir, new TransactedDirectory(dir), true)
    {
    }
}

public class SqliteTransactedDirectoryTest : RootTransactedDirectoryTest
{
    public SqliteTransactedDirectoryTest() : this(DoCreate(new SqliteTransactableStore("")))
    {
    }

    private static SqliteTransactableStore DoCreate(SqliteTransactableStore store)
    {
        using var dir = store.BeginTransaction();
        dir.Create();
        dir.Commit();
        return store;
    }

    private SqliteTransactedDirectoryTest(SqliteTransactableStore dir) : 
        base(dir.UntransactedRoot, dir.BeginTransaction(), false)
    {
    }
}

public abstract class RootTransactedDirectoryTest(IDirectory rawTarget, ITransactedDirectory dir, bool supportsIsolation)
{
    
    [Fact]
    public async Task CommitCreatesFile()
    {
        dir.File("a.b").Create("value");
        if (supportsIsolation) Assert.False(rawTarget.File("a.b").Exists());
        await dir.Commit();
        Assert.True(rawTarget.File("a.b").Exists());
        rawTarget.File("a.b").AssertContent("value");                                                                                                                               
        Assert.Single(rawTarget.AllFiles());
    }
    [Fact]
    public async Task CommitCreatesFileInSubdir()
    {
        var inner = dir.SubDirectory("Wow");
        inner.Create();
        inner.File("a.b").Create("value");
        if (supportsIsolation) Assert.False(rawTarget.SubDirectory("Wow").File("a.b").Exists());
        await dir.Commit();
        Assert.True(rawTarget.SubDirectory("Wow").File("a.b").Exists());
        rawTarget.SubDirectory("Wow").File("a.b").AssertContent("value");
        Assert.Single(rawTarget.SubDirectory("Wow").AllFiles());
    }
    [Fact]
    public void RollbackErasesFile()
    {
        dir.File("a.b").Create("value"); 
        // this line no longer works because Sqlite supports genuine transaction isolation.
//        Assert.Single(rawTarget.AllFiles());
        if (supportsIsolation) Assert.False(rawTarget.File("a.b").Exists());
        dir.Rollback();
        Assert.False(rawTarget.File("a.b").Exists());
        Assert.Empty(rawTarget.AllFiles());
    }
}