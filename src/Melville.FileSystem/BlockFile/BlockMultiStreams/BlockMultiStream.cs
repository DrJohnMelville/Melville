using System;
using System.Buffers;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Melville.FileSystem.BlockFile.ByteSinks;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public class BlockMultiStream(
    IByteSink bytes,
    int blockSize = 4096,
    uint freeListHead = 0) : IDisposable
{
    public int BlockSize { get; } = blockSize;
    public int BlockDataSize => blockSize - nextBlockTagSize;
    private volatile uint nextBlock = currentBlocksOnDisk(bytes, blockSize);

    private static uint currentBlocksOnDisk(IByteSink bytes, int blockSize) =>
        (uint)((bytes.Length + blockSize - 1) / blockSize);

    private volatile uint freeListHead = freeListHead;

    public static async Task<BlockMultiStream> CreateFrom(IByteSink bytes)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(8);
        try
        {
            await bytes.ReadExactAsync(buffer.AsMemory(0, 8), 0);
            var sizes = MemoryMarshal.Cast<byte, uint>(buffer.AsSpan(0, 8));
            return new BlockMultiStream(bytes, (int)sizes[0], sizes[1]);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public void Dispose() => bytes.Dispose();

    public Stream GetReader(uint firstBlock, long streamLength) =>
        new BlockStreamReader(this, firstBlock, streamLength);

    internal ValueTask<int> ReadFromBlockDataAsync(
        Memory<byte> target, uint block, int offset) =>
        bytes.ReadAsync(target.OfMaxLen(DataRemainingInBlock(offset)),
            PositionForDataInBlock(block, offset));

    public async Task<int> WriteToBlockDataAsync(
        ReadOnlyMemory<byte> buffer, uint block, int offset)
    {
        var len = Math.Min(DataRemainingInBlock(offset), buffer.Length);
        await bytes.WriteAsync(buffer.OfMaxLen(len), PositionForDataInBlock(block, offset));
        return len;
    }


    private const int nextBlockTagSize = 4;

    private long PositionForDataInBlock(uint block, int offset) =>
        (block * BlockSize) + offset;

    internal int DataRemainingInBlock(int offset) => BlockDataSize - offset;

    public async Task<uint> NextBlockForAsync(uint currentBlock)
    {
        var dataLocation = PositionForDataInBlock(currentBlock + 1, -4);
        var buffer = ArrayPool<byte>.Shared.Rent(nextBlockTagSize);
            await bytes.ReadExactAsync(buffer.AsMemory(0, nextBlockTagSize), dataLocation);
            var ret = MemoryMarshal.Cast<byte, uint>(buffer)[0];
            if (ret == 0)
                throw new IOException("Attempt to read or seek off the end of the block list.");
            ArrayPool<byte>.Shared.Return(buffer);
            return ret;
    }
    public uint NextBlockFor(uint currentBlock)
    {
        var dataLocation = PositionForDataInBlock(currentBlock + 1, -4);
        Span<byte> buffer = stackalloc byte[nextBlockTagSize];
        bytes.ReadExact(buffer, dataLocation);
        var ret = MemoryMarshal.Cast<byte, uint>(buffer)[0];
        if (ret == 0)
            throw new IOException("Attempt to read or seek off the end of the block list.");
        return ret;
    }

    public async Task<Stream> GetWriterAsync() => 
        new BlockWritingStream(this, await NextFreeBlockAsync());

    private readonly SemaphoreSlim freeBlockMutex = new SemaphoreSlim(1); 
    
    public async Task<uint> NextFreeBlockAsync()
    {
        await freeBlockMutex.WaitAsync();
        try
        {
            return Interlocked.Increment(ref nextBlock);
        }
        finally
        {
            freeBlockMutex.Release();
        }
    }

    public uint NextFreeBlock()
    {
        freeBlockMutex.Wait();
        try
        {
            return Interlocked.Increment(ref nextBlock);
        }
        finally
        {
            freeBlockMutex.Release();
        }
    }

    public async Task WriteNextBlockLinkAsync(uint currentBlock, uint next)
    {
        var dataLocation = PositionForDataInBlock(currentBlock + 1, -4);
        var buffer = ArrayPool<byte>.Shared.Rent(nextBlockTagSize);
        try
        {
            MemoryMarshal.Cast<byte, uint>(buffer.AsSpan())[0] = next;
            await bytes.WriteAsync(buffer.AsMemory(0, 4), dataLocation);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public void WriteNextBlockLink(uint currentBlock, uint u)
    {
        var dataLocation = PositionForDataInBlock(currentBlock + 1, -4);
        Span<byte> buffer = stackalloc byte[nextBlockTagSize];
        MemoryMarshal.Cast<byte, uint>(buffer)[0] = u;
        bytes.Write(buffer, dataLocation);
    }

    public void Flush() => bytes.Flush();
}