using System.Threading.Tasks;
using Melville.FileSystem.PseudoTransactedFS;
using Melville.Mvvm.TestHelpers.MockFiles;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.PseudoTransactedFS;

public class TransactedDirectoryTest
{
    private readonly MockDirectory rawTarget = new MockDirectory("j:\\dir");
    private readonly TransactedDirectory dir;

    public TransactedDirectoryTest()
    {
        dir = new TransactedDirectory(rawTarget);
    }
    
    [Fact]
    public async Task CommitCreatesFile()
    {
        dir.File("a.b").Create("value");
        var target = rawTarget.File("a.b");
        Assert.False(target.Exists());
        await dir.Commit();
        Assert.True(target.Exists());
        target.AssertContent("value");
        Assert.Single(rawTarget.AllFiles());
    }
    [Fact]
    public async Task CommitCreatesFileInSubdir()
    {
        var inner = dir.SubDirectory("Wow");
        inner.Create();
        inner.File("a.b").Create("value");
        var target = rawTarget.SubDirectory("Wow").File("a.b");
        Assert.False(target.Exists());
        await dir.Commit();
        Assert.True(target.Exists());
        target.AssertContent("value");
        Assert.Single(rawTarget.SubDirectory("Wow").AllFiles());
    }
    [Fact]
    public void RollbackErasesFile()
    {
        dir.File("a.b").Create("value");
        var target = rawTarget.File("a.b");
        Assert.Single(rawTarget.AllFiles());
        Assert.False(target.Exists());
        dir.Rollback();
        Assert.False(target.Exists());
        Assert.Empty(rawTarget.AllFiles());
    }
}