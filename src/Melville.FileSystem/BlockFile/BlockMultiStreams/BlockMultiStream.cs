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
    uint blockSize = 4096,
    uint freeListHead = 0xFFFFFFFF,
    uint rootBlock = 0xFFFFFFFF,
    uint nextBlock = 0) : IDisposable
{
    public uint BlockSize { get; } = blockSize;
    public uint RootBlock { get; private set; } = rootBlock;
    public uint BlockDataSize => blockSize - nextBlockTagSize;
    private volatile uint nextBlock = nextBlock;
    private volatile uint freeListHead = freeListHead;

    public static async Task<BlockMultiStream> CreateFrom(IByteSink bytes)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(16);
        try
        {
            var bufferMem = buffer.AsMemory(0, 16);
            await bytes.ReadExactAsync(bufferMem, 0);
            var sizes = MemoryMarshal.Cast<byte, uint>(bufferMem.Span);
            return new BlockMultiStream(bytes, sizes[0], sizes[1], sizes[2], sizes[3]);
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
        var len = (int)Math.Min(DataRemainingInBlock(offset), buffer.Length);
        await bytes.WriteAsync(buffer.OfMaxLen(len), PositionForDataInBlock(block, offset));
        return len;
    }


    private const int nextBlockTagSize = 4;
    private const long headerSize = 16;

    private long PositionForDataInBlock(uint block, int offset) =>
        (block * BlockSize) + offset + headerSize;

    private long PositionForNextBlockLink(uint block) =>
        PositionForDataInBlock(block+1, -nextBlockTagSize);

    internal int DataRemainingInBlock(int offset) => (int)BlockDataSize - offset;
    
    public async Task<uint> NextBlockForAsync(uint currentBlock)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(nextBlockTagSize);
            await bytes.ReadExactAsync(
                buffer.AsMemory(0, nextBlockTagSize), PositionForNextBlockLink(currentBlock));
            var ret = MemoryMarshal.Cast<byte, uint>(buffer)[0];
            ArrayPool<byte>.Shared.Return(buffer);
            return ret;
    }
    public uint NextBlockFor(uint currentBlock)
    {
        var dataLocation = PositionForNextBlockLink(currentBlock);
        Span<byte> buffer = stackalloc byte[nextBlockTagSize];
        bytes.ReadExact(buffer, dataLocation);
        var ret = MemoryMarshal.Cast<byte, uint>(buffer)[0];
        return ret;
    }

    public async Task<BlockWritingStream> GetWriterAsync() => 
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
        var buffer = ArrayPool<byte>.Shared.Rent(nextBlockTagSize);
        try
        {
            MemoryMarshal.Cast<byte, uint>(buffer.AsSpan())[0] = next;
            await bytes.WriteAsync(buffer.AsMemory(0, 4), PositionForNextBlockLink(currentBlock));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public void WriteNextBlockLink(uint currentBlock, uint next)
    {
        Span<byte> buffer = stackalloc byte[nextBlockTagSize];
        MemoryMarshal.Cast<byte, uint>(buffer)[0] = next;
        bytes.Write(buffer, PositionForNextBlockLink(currentBlock));
    }

    public void Flush() => bytes.Flush();
}