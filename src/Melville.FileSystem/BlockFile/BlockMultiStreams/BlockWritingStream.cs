using System;
using System.Threading;
using System.Threading.Tasks;
using Melville.INPC;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public interface IEndBlockDataTarget
{
    void EndStreamWrite(in StreamEnds ends, long length);
    public void EndStreamRead();
}

[StaticSingleton]
public partial class NullEndBlockDataTarget : IEndBlockDataTarget
{
    /// <inheritdoc />
    public void EndStreamWrite(in StreamEnds ends, long length)
    {
    }

    public void EndStreamRead()
    {
    }
}

public class BlockWritingStream(BlockMultiStream data, uint firstBlock, IEndBlockDataTarget dataTarget)
    : BlockStream(data, firstBlock, 0)
{
    public override bool CanWrite => true;
    public override bool CanRead => false;

    public override void SetLength(long value)
    {
        Data.HintIntendedWriteSize(value);
    }

    /// <inheritdoc />
    public override int Read(Span<byte> buffer) => 
        throw new NotSupportedException("This is a writing stream");

    /// <inheritdoc />
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = new CancellationToken()) => 
        throw new NotSupportedException("This is a writing stream");


    public override void Write(ReadOnlySpan<byte> buffer)
    {
        while (buffer.Length > 0)
        {
            EnsureCurrentBlockForPosition();
           var bytesWritten = Data.WriteToBlockData(buffer, CurrentBlock, CurrentBlockOffset);
           Position += bytesWritten;
           buffer = buffer[bytesWritten..];
           TryUpdateLength();
        }
    }

    public override async ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        while (buffer.Length > 0)
        {
            await EnsureCurrentBlockForPositionAsync();
            var bytesWritten = await Data.WriteToBlockDataAsync(
                buffer, CurrentBlock, CurrentBlockOffset);
            Position += bytesWritten;
            buffer = buffer[bytesWritten..];
            TryUpdateLength();
        }
    }

    public override void Flush()
    {
        Data.Flush();
    }

    public override Task FlushAsync(System.Threading.CancellationToken cancellationToken)
    {
        Flush();
        return Task.CompletedTask;
    }

    bool hasDisposed = false;
    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (hasDisposed) return;
        hasDisposed = true;
        dataTarget?.EndStreamWrite(CurrentExtent(), Length);
        base.Dispose(disposing);
    }
}