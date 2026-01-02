using Melville.FileSystem.BlockFile.ByteSinks;
using Melville.INPC;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PddJpegExtractor;
internal class BlockRecoveryStream: Stream
{
    private readonly int blockSize;
    private int BlockDiskSize => blockSize + 4;
    private readonly byte[] buffer;
    private int currentPosition;
    private uint nextBlock = 0;
    private readonly IByteSink sink;

    public BlockRecoveryStream(
        IByteSink sink, uint nextBlock, int blockSize = 4096)
    {
        this.sink = sink;
        this.nextBlock = nextBlock;
        this.blockSize = blockSize;
        buffer = new byte[BlockDiskSize];
        currentPosition = BlockDiskSize;
    }

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => sink.Length;

    public override long Position { get; set; }

    public override int Read(byte[] destination, int offset, int count) =>
        Read(destination.AsSpan(offset, count));
    public override int Read(Span<byte> buffer)
    {
        var totalRead = 0;
        while (buffer.Length > 0)
        {
            if (currentPosition >= blockSize)
            {
                if (!ReadNextBlock()) return totalRead;
            }
            var toCopy = Math.Min(buffer.Length, blockSize - currentPosition);
            this.buffer.AsSpan(currentPosition, toCopy).CopyTo(buffer);
            buffer = buffer.Slice(toCopy);
            totalRead += toCopy;
            currentPosition += toCopy;
            Position += toCopy;
        }
        return totalRead;
    }

    private bool ReadNextBlock()
    {
        var location = 16 + (nextBlock * BlockDiskSize);
        sink.Read(buffer, location);
        nextBlock = MemoryMarshal.Cast<byte, uint>(buffer.AsSpan()[^4..])[0];
        currentPosition = 0;
        return true;
    }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }
}
