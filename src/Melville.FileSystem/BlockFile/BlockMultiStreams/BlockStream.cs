using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public abstract class BlockStream : Stream
{
    protected BlockMultiStream Data { get; }
    public uint FirstBlock => blocks[0];
    public uint CurrentBlock => blocks[(int)(Position / Data.BlockDataSize)];
    private long length;

    private readonly List<uint> blocks = new();

    public BlockStream(BlockMultiStream data, uint firstBlock, long length)
    {
        Data = data;
        blocks.Add(firstBlock);
        this.length = length;
    }

    public override long Length => this.length;
    public override long Position { get; set; }



    protected int CurrentBlockOffset => (int)(Position % Data.BlockDataSize);
    protected int DataRemainingInBlock => Data.DataRemainingInBlock(CurrentBlockOffset);

    public StreamEnds CurrentExtent() => new StreamEnds(blocks[0], blocks[^1]);

    #region Overrides to the memory and span based overloads

    public override int Read(byte[] buffer, int offset, int count) =>
        Read(buffer.AsSpan(offset, count));

    public override void Write(byte[] buffer, int offset, int count) =>
        Write(buffer.AsSpan(offset, count));

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
        Position = ComputeAbsolutePosition(offset, origin);
        // We do not need to ensure the current position has a block because read or write will do that for us,
        // and we want to avoid doing it in a non-async context if the read or write is async.
        // EnsureCurrentBlockForPosition();
        return Position;
    }

    private long ComputeAbsolutePosition(long offset, SeekOrigin origin) => origin switch
    {
        SeekOrigin.Begin => offset,
        SeekOrigin.Current => Position + offset,
        SeekOrigin.End => Length + offset,
        _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
    };

    protected void EnsureCurrentBlockForPosition()
    {
        while (!CurrentPositionHasKnownBlock())
        {
            if (PositionDoesNotExistYet())
            {
                var nextBlock = Data.NextFreeBlock();
                Data.WriteNextBlockLink(blocks[^1], nextBlock);
                blocks.Add(nextBlock);
            }
            else
            {
                blocks.Add(Data.NextBlockFor(blocks[^1]));
            }
        }
    }
    protected async ValueTask EnsureCurrentBlockForPositionAsync()
    {
        while (!CurrentPositionHasKnownBlock())
        {
            if (PositionDoesNotExistYet())
            {
                var nextBlock = await Data.NextFreeBlockAsync();
                await Data.WriteNextBlockLinkAsync(blocks[^1], nextBlock);
                blocks.Add(nextBlock);
            }
            else
            {
                blocks.Add(await Data.NextBlockForAsync(blocks[^1]));
            }
        }
    }

    private bool PositionDoesNotExistYet() => Position >= Length;

    private bool CurrentPositionHasKnownBlock() => Position < FirstByteWithNoBlock;
    private long FirstByteWithNoBlock => (blocks.Count * Data.BlockDataSize);

    #endregion

    protected void TryUpdateLength()=> length = Math.Max(length, Position);
}