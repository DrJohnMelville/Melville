using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public abstract class BlockStream(BlockMultiStream data, uint firstBlock, long length)
    : Stream
{
    protected BlockMultiStream Data { get; } = data;
    protected uint FirstBlock { get; } = firstBlock;
    protected uint CurrentBlock { get; set; } = firstBlock;
    protected long PriorLength { get; set; } = 0;
    private long length = length;
    public override long Length => this.length;
    public override long Position { get; set; }

    protected int CurrentBlockOffset => (int)(Position - PriorLength);
    protected int DataRemainingInBlock => Data.DataRemainingInBlock(CurrentBlockOffset);

    #region Overrides to the memory and span based overloads

    public override int Read(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException("Use ReadAsync instead");

    public override void Write(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException("Use WriteAsync instead.");

    /// <inheritdoc />
    public override Task<int> ReadAsync(
        byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
        ReadAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();

    /// <inheritdoc />
    public override Task WriteAsync(
        byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
        WriteAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();

    #endregion

    #region Seek

    public override bool CanSeek => true;
    public override long Seek(long offset, SeekOrigin origin)
    {
        var absolutePosition = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => Position + offset,
            SeekOrigin.End => Length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
        };
        AbsoluteSeek(absolutePosition);
        return absolutePosition;
    }

    private void AbsoluteSeek(long position)
    {
        if (position < 0 || position > Length)
            throw new ArgumentOutOfRangeException(nameof(position), position, "Position out of range.");

        if (position < PriorLength)
        {
            CurrentBlock = FirstBlock;
            PriorLength = 0;
        }
        SeekForwardTo(position);
    }

    private void SeekForwardTo(long position)
    {
        while (position >= PriorLength + data.BlockDataSize) AdvanceBlock();
        Position = position;
    }

    protected virtual async ValueTask AdvanceBlockAsync()
    {
        PriorLength += Data.BlockDataSize;
        CurrentBlock = await Data.NextBlockForAsync(CurrentBlock);
    }
    protected virtual void AdvanceBlock()
    {
        PriorLength += Data.BlockDataSize;
        CurrentBlock = Data.NextBlockFor(CurrentBlock);
    }
    #endregion

    protected void TryUpdateLength()=> length = Math.Max(length, Position);
}