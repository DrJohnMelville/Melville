using System;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public interface IEndBlockWriteDataTarget
{
    void EndStreamWrite(uint startBlock, uint endBlock, long length);
}

public class BlockWritingStream(BlockMultiStream DATA, uint firstBlock, IEndBlockWriteDataTarget dataTargt)
    : BlockStream(DATA, firstBlock, 0)
{
    public override bool CanWrite => true;
    public override bool CanRead => false;

    public override void SetLength(long value)
    {
        // ignore hints about the length being set, but we can
        // write off the end of the stream to expand the length
    }
    public override async ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        while (buffer.Length > 0)
        {
            if (DataRemainingInBlock <= 0) await AdvanceBlockAsync();
            var bytesWritten = await Data.WriteToBlockDataAsync(
                buffer, CurrentBlock, CurrentBlockOffset);
            Position += bytesWritten;
            buffer = buffer[bytesWritten..];
        }

        TryUpdateLength();
    }

    protected override async ValueTask AdvanceBlockAsync()
    {
        if (Position < Length) await base.AdvanceBlockAsync();
        else
        {
            var nextBlock = await Data.NextFreeBlockAsync();
            await Data.WriteNextBlockLinkAsync(CurrentBlock, nextBlock);
            CurrentBlock = nextBlock;
            PriorLength = Position;
        }
    }
    protected override void AdvanceBlock()
    {
        if (Position < Length) base.AdvanceBlock();
        else
        {
            var nextBlock = Data.NextFreeBlock();
            Data.WriteNextBlockLink(CurrentBlock, nextBlock);
            CurrentBlock = nextBlock;
            PriorLength = Position;
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

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        dataTargt.EndStreamWrite(FirstBlock, CurrentBlock, Length);
        base.Dispose(disposing);
    }
}