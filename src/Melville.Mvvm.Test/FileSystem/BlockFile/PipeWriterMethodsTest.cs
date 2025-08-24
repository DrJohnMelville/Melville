using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using FluentAssertions;
using Melville.FileSystem.BlockFile.FileSystemObjects;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.BlockFile;

public class PipeWriterMethodsTest
{
    private readonly MemoryStream buffer = new();
    private readonly PipeWriter writer;
    private readonly PipeReader reader;

    public PipeWriterMethodsTest()
    {
        writer = PipeWriter.Create(buffer);
        reader = PipeReader.Create(buffer);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(16)]
    [InlineData(253)]
    [InlineData(254)]
    [InlineData(255)]
    [InlineData(256)]
    [InlineData(ushort.MaxValue - 1)]
    [InlineData(ushort.MaxValue - 0)]
    [InlineData(ushort.MaxValue + 1)]
    [InlineData(ushort.MaxValue + 10000)]
    public async Task RoundTripUint(uint item)
    { 
        writer.WriteCompactUint(item);
        await writer.FlushAsync();
        buffer.Seek(0, SeekOrigin.Begin);
        (await reader.ReadCompactUint()).Should().Be(item);
    }
}