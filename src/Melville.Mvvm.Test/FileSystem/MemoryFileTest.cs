using System.IO;
using System.Text;
using System.Threading.Tasks;
using Melville.FileSystem;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem;

public sealed class MemoryFileTest
{
    [Fact]
    public void PathTest()
    {
        Assert.Equal("xxyy", new MemoryFile("xxyy").Path);
    }

    [Fact]
    public void FinalProgressTest()
    {
        Assert.Equal(255, new MemoryFile("xxyy").FinalProgress);
    }
    [Fact]
    public void WaitForFinalTest()
    {
        Assert.True(new MemoryFile("xxyy").WaitForFinal.IsCompleted);
    }

    [Fact]
    public void FileSystemPathTest()
    {
        Assert.False(new MemoryFile("xxyy").ValidFileSystemPath());
    }
    [Fact]
    public void ParentDir()
    {
        Assert.NotNull(new MemoryFile("xxyy").Directory);
    }

    [Fact]
    public async Task ExistaBetweenCreateAndDelete()
    {
        var file = new MemoryFile("xxyy");
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
        var file = new MemoryFile("xxyy");
        using (var str = await file.CreateWrite(FileAttributes.Normal))
        {
            str.Write(Encoding.UTF8.GetBytes("Foo bar"));
        }
        using (var rstr = await file.OpenRead())
        {
            var buf = new byte[40];
            int count = buf.Length;
            var len = rstr.FillBuffer(buf, (int) 0, count);
            Assert.Equal("Foo bar", Encoding.UTF8.GetString(buf, 0, len));
        }
    }
}
