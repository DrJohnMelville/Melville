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
        var sut = await SingleBlockStream("08000000 0A000000 FFFFFFFF 01000000 | 00000000");
        sut.BlockSize.Should().Be(8);
        sut.GetField("nextBlock").Should().Be(1);
        sut.GetField("freeListHead").Should().Be(10);
        sut.RootBlock.Should().Be(uint.MaxValue);
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
        var sut = await SingleBlockStream("08000000 0A000000 FFFFFFFF 00000000 | 00010203");
        var buffer = new byte[4];
        await using var reader = sut.GetReader(0, streamLength);
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
        08000000 00000000 FFFFFFFF 00000000
        00010203 01000000
        04050607 00000000
        """);
        var buffer = new byte[6];
        await using var reader = sut.GetReader(0, 6);
        await reader.ReadExactlyAsync(buffer);
        buffer.Should().BeEquivalentTo([0, 1, 2, 3, 4, 5]);
    }
    [Fact]
    public async Task ReadInvertedBlockStreamAsync()
    {
        var sut = await SingleBlockStream("""
        08000000 00000000 FFFFFFFF 00000000
        04050607 FFFFFFFF
        00010203 00000000
        
        """);
        var buffer = new byte[6];
        await using var reader = sut.GetReader(1, 6);
        await reader.ReadExactlyAsync(buffer);
        buffer.Should().BeEquivalentTo([0, 1, 2, 3, 4, 5]);
    }

    [Fact]
    public async Task SeekWithinBlock()
    {
        var sut = await SingleBlockStream("""
            08000000 00000000 FFFFFFFF 00000000
            04050607 FFFFFFFF
            00010203 00000000

            """);
        await using var reader = sut.GetReader(1, 6);
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
            08000000 00000000 FFFFFFFF 00000000
            04050607 FFFFFFFF
            00010203 00000000

            """);
        await using var reader = sut.GetReader(1, 6);
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
            08000000 00000000 FFFFFFFF 00000000
            04050607 FFFFFFFF
            00010203 00000000

            """);
        await using var reader = sut.GetReader(1, 6);
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
        await writer.WriteAsync(data1);
        await VerifyStream(sut, writer.FirstBlock, data1);
    }
    private static readonly byte[] data1 = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private static readonly byte[] data2 = new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
    [Fact]
    public async Task RoundTripTwoBlockStream()
    {
        var sut = await WriteInterleavedStreams(data1, data2);

        await VerifyStream(sut, 0, data1);
        await VerifyStream(sut, 1, data2);
    }

    private static async Task VerifyStream(BlockMultiStream sut, uint firstBlock, byte[] data)
    {
        var reader = sut.GetReader(firstBlock, data.Length);
        var readData = new byte[data.Length];
        await reader.ReadExactlyAsync(readData);
        readData.Should().BeEquivalentTo(data);
    }

    private static async Task<BlockMultiStream> WriteInterleavedStreams(
        byte[] source1, byte[] source2, MemoryBytesSink? store = null)
    {
        var bytes = store ?? new MemoryBytesSink([]);
        var sut = new BlockMultiStream(bytes, 8);
        var writer1 = await sut.GetWriterAsync();
        var writer2 = await sut.GetWriterAsync();
        for (int i = 0; i < BlockMultiStreamTest.data1.Length; i++)
        {
            await writer1.WriteAsync(source1.AsMemory(i, 1));
            await writer2.WriteAsync(source2.AsMemory(i, 1));
        }

        return sut;
    }

    [Fact]
    public async Task WriteHeaderTest()
    {
        var sink = new MemoryBytesSink([]);
        var sut = await WriteInterleavedStreams(data1, data2, sink);
        await sut.WriteHeaderBlockAsync(0xDEADBEEF);
        sink.Data[..16].Should().BeEquivalentTo(
            "08000000 FFFFFFFF EFBEADDE 06000000".BytesFromHexString());
    }

    [Fact]
    public async Task DeleteChainTest()
    {
        var sink = new MemoryBytesSink([]);
        var sut = await WriteInterleavedStreams(data1, data2, sink);
        await sut.DeleteStreamAsync(0,4);
        await sut.WriteHeaderBlockAsync(0xDEADBEEF);
        sink.Data[..16].Should().BeEquivalentTo(
            "08000000 00000000 EFBEADDE 06000000".BytesFromHexString());

    }
    [Fact]
    public async Task DeleteChainTest2()
    {
        var sink = new MemoryBytesSink([]);
        var sut = await WriteInterleavedStreams(data1, data2, sink);
        await sut.DeleteStreamAsync(1,5);
        await sut.WriteHeaderBlockAsync(0xDEADBEEF);
        sink.Data[..16].Should().BeEquivalentTo(
            "08000000 01000000 EFBEADDE 06000000".BytesFromHexString());

    }
}