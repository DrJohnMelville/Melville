using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Melville.FileSystem;
using Melville.FileSystem.BlockFile.BlockMultiStreams;
using Melville.FileSystem.BlockFile.ByteSinks;
using Melville.FileSystem.BlockFile.FileSystemObjects;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.BlockFile;

public class BlockDirectoryWriterTest
{
    private readonly BlockMultiStream mus = new BlockMultiStream(
        new MemoryBytesSink());
    private readonly BlockRootDirectory root;
    private readonly MemoryStream names = new MemoryStream();
    private readonly MemoryStream blocks = new MemoryStream();
    private readonly FullBlockDirectoryTarget sut;


    public BlockDirectoryWriterTest()
    {
        root = new BlockRootDirectory(mus);
        sut = new FullBlockDirectoryTarget(names, blocks, 0xAAAAAAAA);
    }

    [Fact]
    public async Task WriteEmptyDirectory()
    {
        await root.WriteToAsync(sut);
        await sut.FlushAsync();
        names.ToArray().Should().BeEquivalentTo([0, 0]);
        blocks.ToArray().Should().BeEquivalentTo(
            "AAAAAAAA".BytesFromHexString());
    }

    [Fact]
    public async Task WriteSingleFileWithName()
    {
        await CreateFile(root, "ABC");

        await root.WriteToAsync(sut);
        await sut.FlushAsync();

        names.ToArray().Should().BeEquivalentTo(
            "00 01 03 41 42 43".BytesFromHexString());

        blocks.ToArray().Should().BeEquivalentTo(
            "AAAAAAAA 00000000 00000000 01000000_00000000".BytesFromHexString());

    }

    private async Task CreateFile(IDirectory directory, string name)
    {
        await using (var f = await directory.File(name).CreateWrite())
        {
            await f.WriteAsync([0x42],0,1);
        }
    }

    [Fact]
    public async Task WriteTwoFilesWithName()
    {
        await CreateFile(root, "A");
        await CreateFile(root, "B");

        await root.WriteToAsync(sut);
        await sut.FlushAsync();

        names.ToArray().Should().BeEquivalentTo(
            "00 02 01 41 01 42".BytesFromHexString());

        blocks.ToArray().Should().BeEquivalentTo(
            """
            AAAAAAAA 
            00000000 00000000 01000000_00000000
            01000000 01000000 01000000_00000000
            """.BytesFromHexString());
    }
    [Fact]
    public async Task WriteTwoFilesInSubFolder()
    {
        var subDir = root.SubDirectory("C");
        await CreateFile(subDir, "A");
        await CreateFile(subDir, "B");

        await root.WriteToAsync(sut);
        await sut.FlushAsync();

        names.ToArray().Should().BeEquivalentTo(
            """
            01
            01 43
            00 
            02 
            01 41 
            01 42
            00
            """.BytesFromHexString());

        blocks.ToArray().Should().BeEquivalentTo(
            """
            AAAAAAAA 
            00000000 00000000 01000000_00000000
            01000000 01000000 01000000_00000000
            """.BytesFromHexString());
    }
}