using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentAssertions;
using Melville.FileSystem.BlockFile.BlockMultiStreams;
using Melville.FileSystem.BlockFile.ByteSinks;
using Melville.Hacks.Reflection;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.BlockFile;

public class BlockMultiStreamTest
{
    [Fact]
    public async Task ReadMultiStreamTest()
    {
        var sut = await SingleBlockStream("08000000 0A000000 |00010203 00000000");
        sut.BlockSize.Should().Be(8);
        sut.GetField("nextBlock").Should().Be(2);
        sut.GetField("freeListHead").Should().Be(10);
    }

    private static async Task<BlockMultiStream> SingleBlockStream(string data)
    {
        var source = data.BytesFromHexString();
        var sut = await BlockMultiStream.CreateFrom(
            new MemoryBytesSink(source));
        return sut;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task ReadSingleBlockStream(long streamLength)
    {
        var sut = await SingleBlockStream("08000000 0A000000 |00010203 00000000");
        var buffer = new byte[4];
        await using var reader = sut.GetReader(1, streamLength);
        reader.CanRead.Should().BeTrue();
        reader.CanSeek.Should().BeTrue();
        reader.CanWrite.Should().BeFalse();
        await reader.ReadAtLeastAsync(buffer, 4, false);
        for (int i = 0; i < 4; i++)
        {
            if (i < streamLength)
            {
                buffer[i].Should().Be((byte)i);
            }
            else
            {
                buffer[i].Should().Be(0);
            }
        }
    }

    [Fact]
    public async Task ReadTwoBlockStreamAsync()
    {
        var sut = await SingleBlockStream("""
        08000000 00000000
        00010203 02000000
        04050607 00000000
        """);
        var buffer = new byte[6];
        await using var reader = sut.GetReader(1, 6);
        await reader.ReadExactlyAsync(buffer);
        buffer.Should().BeEquivalentTo([0, 1, 2, 3, 4, 5]);
    }
    [Fact]
    public async Task ReadInvertedBlockStreamAsync()
    {
        var sut = await SingleBlockStream("""
        08000000 00000000
        04050607 00000000
        00010203 01000000
        
        """);
        var buffer = new byte[6];
        await using var reader = sut.GetReader(2, 6);
        await reader.ReadExactlyAsync(buffer);
        buffer.Should().BeEquivalentTo([0, 1, 2, 3, 4, 5]);
    }

    [Fact]
    public async Task SeekWithinBlock()
    {
        var sut = await SingleBlockStream("""
            08000000 00000000
            04050607 00000000
            00010203 01000000

            """);
        await using var reader = sut.GetReader(2, 6);
        reader.CanSeek.Should().BeTrue();
        reader.Seek(3, SeekOrigin.Begin);
        var buffer = new byte[2];
        await reader.ReadExactlyAsync(buffer);
        buffer.Should().BeEquivalentTo([3, 4]);
    }
    [Fact]
    public async Task SeekForward()
    {
        var sut = await SingleBlockStream("""
            08000000 00000000
            04050607 00000000
            00010203 01000000

            """);
        await using var reader = sut.GetReader(2, 6);
        reader.CanSeek.Should().BeTrue();
        reader.Seek(5, SeekOrigin.Begin);
        var buffer = new byte[2];
        (await reader.ReadAsync(buffer)).Should().Be(1);
        buffer.Should().BeEquivalentTo([5,0]);
    }
    
    [Fact]
    public async Task SeekBack()
    {
        var sut = await SingleBlockStream("""
            08000000 00000000
            04050607 00000000
            00010203 01000000

            """);
        await using var reader = sut.GetReader(2, 6);
        reader.CanSeek.Should().BeTrue();
        reader.Seek(5, SeekOrigin.Begin);
        reader.Seek(-2, SeekOrigin.Current);
        var buffer = new byte[2];
        (await reader.ReadAsync(buffer)).Should().Be(2);
        buffer.Should().BeEquivalentTo([3,4]);
    }

    [Fact]
    public async Task RoundTripSingleStream()
    {
        var bytes = new MemoryBytesSink([]);
        var sut = new BlockMultiStream(bytes, 8, 0);
        var writer = await sut.GetWriterAsync();
        var data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        await writer.WriteAsync(data);
        var readDate = new byte[10];
        var reader = sut.GetReader(1, 10);
        await reader.ReadExactlyAsync(readDate);
        readDate.Should().BeEquivalentTo(data);
    }
}