using System;
using System.Threading.Tasks;
using FluentAssertions;
using Melville.FileSystem.BlockFile.ByteSinks;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.BlockFile;

public class MemoryByteSyncTest
{
    public readonly MemoryBytesSink sut = new MemoryBytesSink([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);

    [Theory]
    [InlineData(100)]
    [InlineData(2)]
    [InlineData(0)]
    public void LengthTest(int length)
    {
        sut.Length.Should().Be(11);
        sut.Length = length;
        sut.Length.Should().Be(length);
    }

    [Fact]
    public void ReadSyncTest()
    {
        Span<byte> buffer = stackalloc byte[3];
        sut.Read(buffer, 4).Should().Be(3);
        buffer.ToArray().Should().BeEquivalentTo([4, 5, 6]);
    }
    [Fact]
    public async Task ReadAsyncTest()
    {
        byte[] buffer = new byte[3];
        (await sut.ReadAsync(buffer, 4)).Should().Be(3);
        buffer.Should().BeEquivalentTo([4, 5, 6]);
    }
    [Fact]
    public void ReadOffEndSyncTest()
    {
        Span<byte> buffer = stackalloc byte[3];
        sut.Read(buffer, 9).Should().Be(2);
        buffer.ToArray().Should().BeEquivalentTo([9,10, 0]);
    }

    [Fact]
    public void ReadTwoBuffers()
    {
        byte[] buffer1 = new byte[3];
        byte[] buffer2 = new byte[4];
        sut.Read([buffer1, buffer2], 4).Should().Be(7);
        buffer1.Should().BeEquivalentTo([4, 5, 6]);
        buffer2.Should().BeEquivalentTo([7, 8, 9, 10]);
    }
    [Fact]
    public async Task ReadTwoBuffersAsync()
    {
        byte[] buffer1 = new byte[3];
        byte[] buffer2 = new byte[4];
        (await sut.ReadAsync([buffer1, buffer2], 4)).Should().Be(7);
        buffer1.Should().BeEquivalentTo([4, 5, 6]);
        buffer2.Should().BeEquivalentTo([7, 8, 9, 10]);
    }

    [Fact]
    public void ReadTwoBuffersIncompleteSecond()
    {
        byte[] buffer1 = new byte[3];
        byte[] buffer2 = new byte[4];
        sut.Read([buffer1, buffer2], 5).Should().Be(6);
        buffer1.Should().BeEquivalentTo([5, 6,7]);
        buffer2.Should().BeEquivalentTo([8, 9, 10, 0]);
    }
    [Fact]
    public void ReadTwoBuffersIncompleteFirst()
    {
        byte[] buffer1 = new byte[3];
        byte[] buffer2 = new byte[4];
        sut.Read([buffer1, buffer2], 9).Should().Be(2);
        buffer1.Should().BeEquivalentTo([9,10,0]);
        buffer2.Should().BeEquivalentTo([0, 0, 0, 0]);
    }

    [Fact]
    public void Dump() =>
        sut.Dump.Should().StartWith("00000000 00 01 02 03 04 05 06 07 08 09 0A");

    [Fact]
    public void Write()
    {
        sut.Write([1, 2, 3], 4);
        sut.Dump.Should().StartWith("00000000 00 01 02 03 01 02 03 07 08 09 0A");
    }
    [Fact]
    public async Task WriteAsync()
    {
        await sut.WriteAsync(new ReadOnlyMemory<byte>([1, 2, 3]), 4);
        sut.Dump.Should().StartWith("00000000 00 01 02 03 01 02 03 07 08 09 0A");
    }
    [Fact]
    public void WriteOffEnd()
    {
        sut.Write([10, 11, 12], 10);
        sut.Dump.Should().StartWith("00000000 00 01 02 03 04 05 06 07 08 09 0A 0B 0C");
        sut.Length.Should().Be(13);
    }
    [Fact]
    public void WriteCompound()
    {
        var m1 = new ReadOnlyMemory<byte>([1, 2]);
        var m2 = new ReadOnlyMemory<byte>([3]);
        sut.Write([m1,m2], 4);
        sut.Dump.Should().StartWith("00000000 00 01 02 03 01 02 03 07 08 09 0A");
    }
    [Fact]
    public async Task WriteCompoundAsync()
    {
        var m1 = new ReadOnlyMemory<byte>([1, 2]);
        var m2 = new ReadOnlyMemory<byte>([3]);
        await sut.WriteAsync([m1,m2], 4);
        sut.Dump.Should().StartWith("00000000 00 01 02 03 01 02 03 07 08 09 0A");
    }
}