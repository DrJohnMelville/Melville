#nullable disable warnings
using System;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using FluentAssertions;
using Melville.FileSystem;
using Melville.FileSystem.Sqlite;
using Melville.Mvvm.TestHelpers.MockFiles;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem;

public sealed class DirectoryOperationsTest
{
  private readonly MockDirectory dir = new MockDirectory("m:\\sgfkdnjh");
  [Fact]
  public void CreateRandomName()
  {
    Assert.Matches(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}\.xyz$", dir.GetRandomFile("xyz").Name);
  }
}

// this abstract class is inherited by classes that run these tests on both
// MemoryDirectory and SqliteDirectory.
public abstract class CommonDirectoryOperationsTest(IDirectory sut)
{
    [Fact]
    public void HasName() => sut.Name.Should().Be("TestDir");

    [Fact]
    public void DoesNotExistByDefault() => sut.Exists().Should().BeFalse();

    [Fact]
    public void CreatingADirectoryMakesItExist()
    {
        sut.Exists().Should().BeFalse();
        sut.Create(FileAttributes.Directory | FileAttributes.Archive);
        var now = DateTime.Now;
        sut.Exists().Should().BeTrue();
        sut.Attributes.Should().Be(FileAttributes.Directory | FileAttributes.Archive);
        (sut.Created - now).TotalSeconds.Should().BeLessThan(1);
        (sut.LastWrite - now).TotalSeconds.Should().BeLessThan(1);
    }

    [Fact]
    public void CreateSubDirectory()
    {
        sut.Create();
        var subDir = sut.SubDirectory("Foo");
        subDir.Name.Should().Be("Foo");
        subDir.Path.Should().Be("TestDir\\Foo");
        subDir.Exists().Should().BeFalse();
        subDir.Create();
        subDir.Exists().Should().BeTrue();

        // here we ask again and we should get the existing folder.
        sut.SubDirectory("Foo").Exists().Should().BeTrue();

        var second = subDir.SubDirectory("foo");
        second.Exists().Should().BeFalse();
        second.Create();
        subDir.SubDirectory("FOO").Exists().Should().BeTrue();
    }

    [Fact]
    public void EnumerateSubDirs()
    {
        sut.Create();
        sut.SubDirectory("Foo").Create();
        sut.SubDirectory("Bar");
        sut.SubDirectory("Baz").Create();

        sut.AllSubDirectories().Select(x => x.Name).Should().BeEquivalentTo("Foo", "Baz");
    }

    [Fact]
    public void DeleteSubdir()
    {
        sut.Create();
        var sub = sut.SubDirectory("Foo");
        sub.Exists().Should().BeFalse();
        sub.Create();
        sub.Exists().Should().BeTrue();
        sut.SubDirectory("Foo").Exists().Should().BeTrue();
        sub.Delete();
        sub.Exists().Should().BeFalse();
        sut.SubDirectory("Foo").Exists().Should().BeFalse();
    }

    [Fact]
    public async Task CreateFile()
    {
        sut.Create();
        var file = sut.File("Foo.txt");
        file.Exists().Should().BeFalse();
        var now = DateTime.Now;
        using (var stream = await file.CreateWrite())
        {
        }
        file.Exists().Should().BeTrue();
        (file.LastWrite - now).TotalSeconds.Should().BeLessThan(1);
        (file.Created - now).TotalSeconds.Should().BeLessThan(1);
        sut.File("Foo.txt").Exists().Should().BeTrue();
    }
    [Fact]
    public async Task DeleteFile()
    {
        sut.Create();
        var file = sut.File("Foo.txt");
        await using (var stream = await file.CreateWrite())
        {
        }
        file.Exists().Should().BeTrue();
        file.Delete();
        file.Exists().Should().BeFalse();
        sut.File("Foo.txt").Exists().Should().BeFalse();

    }

    [Fact]
    public async Task AllFilesTest()
    {
        sut.Create();
        foreach (var name in new[]{"Foo.txt", "bar.txt"})
        {
            var file = sut.File(name);
            await using (var stream = await file.CreateWrite())
            {
            }
        }
        sut.AllFiles().Select(x => x.Name).Should().BeEquivalentTo("Foo.txt", "bar.txt");
    }

    [Theory]
    [InlineData("a.txt", "b.txt", false)]
    [InlineData("a.txt", "a.txt", true)]
    [InlineData("*.txt", "a.txt", true)]
    [InlineData("*.txt", "akhfdkhgae.txt", true)]
    [InlineData("a*.txt", "akhfdkhgae.txt", true)]
    [InlineData("k*.txt", "akhfdkhgae.txt", false)]
    public void AllFilesTestWithGlob(string glob, string name, bool exists)
    {
        sut.Create();
        sut.File(name).Create("Content");
        Assert.Equal(exists, sut.AllFiles(glob).Any());
    }
}

public class MemoryDirectoryTests():CommonDirectoryOperationsTest(new MockDirectory("TestDir"));

public class SqliteDirectoryTests() : CommonDirectoryOperationsTest(
    TestSqliteFileSystemCreator.SqliteDirectory("TestDir"));

public static class TestSqliteFileSystemCreator
{
    public static IFile SqliteFile(string dirName, string fileName)
    {
        var dir = SqliteDirectory(dirName);
        dir.Create();
        return dir.File(fileName);
    }

    public static IDirectory SqliteDirectory(string name) =>
        SqliteFileStore.Create().UntransactedRoot(name);
}