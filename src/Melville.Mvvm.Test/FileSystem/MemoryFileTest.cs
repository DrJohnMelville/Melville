using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Melville.FileSystem;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem;

public sealed class MemoryFileTest() : GenericFileTest(new MemoryFile("xxyy"));

public sealed class SqliteFileTest() : GenericFileTest(
    TestSqliteFileSystemCreator.SqliteFile("", "xxyy"))
{
}

public abstract class GenericFileTest(IFile sut)
{
    [Fact]
    public void PathTest() => sut.Path.Should().EndWith("xxyy");

    [Fact]
    public void FinalProgressTest() => Assert.Equal(255, sut.FinalProgress);

    [Fact]
    public void WaitForFinalTest() => Assert.True(sut.WaitForFinal.IsCompleted);

    [Fact]
    public void FileSystemPathTest() => Assert.False(sut.ValidFileSystemPath());

    [Fact]
    public void ParentDir() => Assert.NotNull(sut.Directory);

    [Fact]
    public async Task ExistaBetweenCreateAndDelete()
    {
        var file = sut;
        Assert.False(file.Exists());
        using (var str = await file.CreateWrite(FileAttributes.Normal))
        {
            str.Write(Encoding.UTF8.GetBytes("Foo bar"));
        }
        Assert.True(file.Exists());
        file.Delete();
        Assert.False(file.Exists());
    }
    [Fact]
    public async Task ReadFile()
    {
        using (var str = await sut.CreateWrite(FileAttributes.Normal))
        {
            str.Write(Encoding.UTF8.GetBytes("Foo bar"));
        }
        using (var rstr = await sut.OpenRead())
        {
            var buf = new byte[40];
            int count = buf.Length;
            var len = rstr.FillBuffer(buf, (int) 0, count);
            Assert.Equal("Foo bar", Encoding.UTF8.GetString(buf, 0, len));
        }
    }

    [Fact]
    public async Task BigFileRoundTripAsync()
    {
        var size = 10_000;
        byte[] data = Enumerable.Range(0, size).Select(i => (byte)i).ToArray();
        await using (var str = await sut.CreateWrite())
        {
            await str.WriteAsync(data);
        }
        await using (var rstr = await sut.OpenRead())
        {
            var buf = new byte[size];
            await rstr.FillBufferAsync(buf, 0, buf.Length);
            buf.Should().BeEquivalentTo(data);
        }
    }

    [Fact]
    public async Task BigFileRoundTrip()
    {
        var size = 10_000;
        byte[] data = Enumerable.Range(0, size).Select(i => (byte)i).ToArray();
        using (var str = await sut.CreateWrite())
        {
            str.Write(data, 0, data.Length);
        }
        using (var rstr = await sut.OpenRead())
        {
            var buf = new byte[size];
            rstr.FillBuffer(buf, 0, buf.Length);
            buf.Should().BeEquivalentTo(data);
        }
    }
}
