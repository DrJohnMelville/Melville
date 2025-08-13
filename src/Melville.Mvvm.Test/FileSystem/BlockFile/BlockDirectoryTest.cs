using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Melville.FileSystem;
using Melville.FileSystem.BlockFile.BlockMultiStreams;
using Melville.FileSystem.BlockFile.ByteSinks;
using Melville.FileSystem.BlockFile.FileSystemObjects;
using Melville.Hacks.Reflection;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.BlockFile;

public class BlockDirectoryTest
{
    private readonly BlockMultiStream mus = new BlockMultiStream(
        new MemoryBytesSink());
    private readonly BlockRootDirectory root;

    public BlockDirectoryTest()
    {
        root = new BlockRootDirectory(mus);
    }

    [Fact]
    public void BlockDirectoryHasNameAndPath()
    {
        var dir = root.SubDirectory("Dir");
        root.SubDirectory("Dir").Should().BeSameAs(dir);

        dir.Directory.Should().Be(root);
        dir.ValidFileSystemPath().Should().BeFalse();
        dir.LastAccess.Should().Be(default);
        dir.LastWrite.Should().Be(default);
        dir.Created.Should().Be(default);
        dir.Exists().Should().BeTrue();
        dir.Attributes.Should().Be(FileAttributes.Directory);

    }

    [Fact]
    public async Task CreateFile()
    {
        var file = root.File("File.txt");
        file.Name.Should().Be("File.txt");
        file.Path.Should().Be("/File.txt");
        file.Attributes.Should().Be(FileAttributes.Normal);
        file.FinalProgress.Should().Be(255);
        file.WaitForFinal.Should().Be(Task.CompletedTask);

        file.Exists().Should().BeFalse();
        file.Size.Should().Be(0);
        await (await file.CreateWrite()).DisposeAsync();
        file.Exists().Should().BeTrue();
        file.Size.Should().Be(0);
    }

    [Fact]
    public async Task RoundtripFile()
    {
        var file = root.File("File.txt");
        var data = new byte[] { 1, 2, 3, 4, 5 };
        await using (var stream = await file.CreateWrite())
        {
            await stream.WriteAsync(data);
        }

        await using (var stream = await file.OpenRead())
        {
            var readData = new byte[data.Length];
            (await stream.ReadAsync(readData)).Should().Be(data.Length);
            readData.Should().BeEquivalentTo(data);
        }
    }

    [Fact]
    public async Task DeleteFile()
    {
        var file = root.File("File.txt");
        await using (var stream = await file.CreateWrite())
        {
            await stream.WriteAsync(new byte[] { 1, 2, 3, 4, 5 });
        }
        file.Exists().Should().BeTrue();
        file.Delete();
        file.Exists().Should().BeFalse();
    }

    [Fact]
    public async Task AllFilesTest()
    {
        var f1 = root.File("File1.txt");
        await (await f1.CreateWrite()).DisposeAsync();
        var f2 = root.File("File2.txt");
        await (await f2.CreateWrite()).DisposeAsync();
        var f3 = root.File("File3.txt"); // uncreated files do not get retrned

        root.AllFiles().Should().BeEquivalentTo(new[] { f1, f2 });
        root.AllFiles("File1*").Should().BeEquivalentTo(new[] { f1 });
    }

    [Fact]
    public void SubDirectoryTest()
    {
        var subDir = root.SubDirectory("SubDir");
        var subDir2 = root.SubDirectory("SubDir2");
        root.AllSubDirectories().Should().BeEquivalentTo(
            [subDir, subDir2]);
    }

    [Fact]
    public async Task RoundTripEmpty()
    {
        await RoundTripDirectory();
    }

    private async Task RoundTripDirectory()
    {
        await root.Commit();
        var root2 = new BlockRootDirectory(mus);
        await root2.ReadFromStore();
        root2.GetField("nameLocation").Should().BeEquivalentTo(root.GetField("nameLocation"));
        root2.AllFiles().Should().BeEquivalentTo(root.AllFiles());
    }

    [Fact]
    public async Task RoundTripDirectoryWithFiles()
    {
        var file1 = root.File("File1.txt");
        await (await file1.CreateWrite()).DisposeAsync();
        var file2 = root.File("File2.txt");
        await (await file2.CreateWrite()).DisposeAsync();
        await RoundTripDirectory();
    }
    [Fact]
    public async Task RoundTripDirectoryWithFilesAndSubDirs()
    {
        var file1 = root.File("File1.txt");
        await (await file1.CreateWrite()).DisposeAsync();
        var subDirectory  = root.SubDirectory("SubDir");
        var file2 = subDirectory.File("File2.txt");
        await (await file2.CreateWrite()).DisposeAsync();
        await RoundTripDirectory();
    }
    [Fact]
    public async Task DeleteFileAndSubdir()
    {
        var file1 = root.File("File1.txt");
        await (await file1.CreateWrite()).DisposeAsync();
        var subDirectory  = root.SubDirectory("SubDir");
        var file2 = subDirectory.File("File2.txt");
        await (await file2.CreateWrite()).DisposeAsync();
        await RoundTripDirectory();
        file2.Delete();
        await RoundTripDirectory();
    }

    [Fact] public async Task DeleteFileAndSubdirWithCreation()
    {
        var file1 = root.File("File1.txt");
        await (await file1.CreateWrite()).DisposeAsync();
        var subDirectory  = root.SubDirectory("SubDir");
        var file2 = subDirectory.File("File2.txt");
        await (await file2.CreateWrite()).DisposeAsync();
        await RoundTripDirectory();
        file2.Delete();
        var File3 = root.File("File3.txt");
        await (await File3.CreateWrite()).DisposeAsync();
        await RoundTripDirectory();
    }
}