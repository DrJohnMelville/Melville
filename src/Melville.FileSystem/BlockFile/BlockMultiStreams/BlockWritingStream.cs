using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public class BlockWritingStream(BlockMultiStream data, uint firstBlock, IEndBlockDataTarget dataTarget)
    : BlockStream(data, firstBlock, 0)
{
    private  BlockMultiStream writableData => (BlockMultiStream)data;
    public override bool CanWrite => true;
    public override bool CanRead => false;

    public override void SetLength(long value) => data.HintIntendedWriteSize(value);

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
           var bytesWritten = data.WriteToBlockData(buffer, CurrentBlock, CurrentBlockOffset);
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
            var bytesWritten = await data.WriteToBlockDataAsync(
                buffer, CurrentBlock, CurrentBlockOffset);
            Position += bytesWritten;
            buffer = buffer[bytesWritten..];
            TryUpdateLength();
        }
    }

    public override void Flush()
    {
        data.Flush();
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

    protected override uint GetNewBlock(uint tail)
    {
        var nextBlock = writableData.NextFreeBlock();
        writableData.WriteNextBlockLink(tail, nextBlock);
        return nextBlock;
    }
    protected override async ValueTask<uint> GetNewBlockAsync(uint tail)
    {
        var nextBlock = await writableData.NextFreeBlockAsync();
        await writableData.WriteNextBlockLinkAsync(tail, nextBlock);
        return nextBlock;

    }

}