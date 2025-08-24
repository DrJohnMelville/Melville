using System;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public class BlockStreamReader(BlockMultiStream data, uint firstBlock, long length)
    : BlockStream(data, firstBlock, length)
{
    public override bool CanRead => true;
    public override bool CanWrite => false;

    public override int Read(Span<byte> buffer)
    {
        // See te comment in ReadAsync about reading at most one block.
        TryAdvanceBlock();
        var localRead = Data.ReadFromBlockData(buffer.OfMaxLen(RemainingLength()), CurrentBlock, CurrentBlockOffset);
        Position += localRead;
        return localRead;
    }

    public override async ValueTask<int>
        ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = new CancellationToken())
    {
        // notice that this reads at most one block, even if the buffer is bigger.  This is because when we are
        // reading the offset and directory name streams, we do not know the length, and buffering was reading off the
        // end of the stream.  Also we should not hit the disk to buffer bytes the consumer may never actually ask
        // for.  The contract for ReadAsync requires only that we put some bytes in the buffer, not that we fill it.
        await TryAdvanceBlockAsync();
            var localRead = await Data.ReadFromBlockDataAsync(
                buffer.OfMaxLen(RemainingLength()), CurrentBlock, CurrentBlockOffset);
            Position += localRead;

        return localRead;
    }

    private long RemainingLength() => Length - Position;

    private ValueTask TryAdvanceBlockAsync() =>
        AnotherBlockNeeded() ? AdvanceBlockAsync() : ValueTask.CompletedTask;
    private void TryAdvanceBlock()
    {
        if (AnotherBlockNeeded())
            AdvanceBlock();
    }

    private bool AnotherBlockNeeded() => DataRemainingInBlock <= 0 && Position < Length;

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Flush()
    {
    }

    public override Task FlushAsync(System.Threading.CancellationToken cancellationToken)
    {
        Flush();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override void Write(ReadOnlySpan<byte> buffer) => 
        throw new NotSupportedException("This is a reading stream");

    /// <inheritdoc />
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = new CancellationToken()) => 
        throw new NotSupportedException("This is a reading stream");
}