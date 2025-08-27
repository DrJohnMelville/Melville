using Melville.FileSystem.BlockFile.ByteSinks;
using Melville.Hacks;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public class BlockMultiStream(
    IByteSink bytes,
    uint blockSize = 4096 + 4, // defaults to 4k of data + 4 bytes for the next block tag
    uint freeListHead = BlockMultiStream.InvalidBlock,
    uint rootBlock = BlockMultiStream.InvalidBlock,
    uint nextBlock = 0) : IDisposable
{
    public const uint InvalidBlock = 0xFFFFFFFF;

    public uint BlockSize { get; } = blockSize;
    public uint RootBlock { get; private set; } = rootBlock;
    public uint BlockDataSize => BlockSize - nextBlockTagSize;
    private uint nextBlock = nextBlock;
    private uint freeListHead = freeListHead;

    private readonly ConcurrentQueue<StreamEnds>
        chainsPendingDelete = new();

    public static async Task<BlockMultiStream> CreateFrom(IByteSink bytes)
    {
        if (bytes.Length is 0)
            return new BlockMultiStream(bytes);
        using var buffer = ArrayPool<byte>.Shared.RentHandle(16);
        var bufferMem = buffer.AsMemory(0, 16);
        await bytes.ReadExactAsync(bufferMem, 0);
        var sizes = MemoryMarshal.Cast<byte, uint>(bufferMem.Span);
        Debug.Assert(sizes[2] != sizes[3]);
        Debug.Assert(sizes[1] != sizes[3]);
        Debug.Assert(sizes[1] != sizes[2]);
        return new BlockMultiStream(bytes, sizes[0], sizes[1], sizes[2], sizes[3]);
    }

    public async Task WriteHeaderBlockAsync(uint rootPosition)
    {
        Debug.Assert(rootPosition != freeListHead);
        Debug.Assert(rootPosition != nextBlock);
        Debug.Assert(freeListHead != nextBlock);
        using var _ = await freeBlockMutex.WaitForHandleAsync();
        await AddDeletedChainsToFreeList();
        RootBlock = rootPosition;
        using var buffer = ArrayPool<byte>.Shared.RentHandle((int)headerSize);
        var innerBuffer = buffer.AsMemory(0, (int)headerSize);
        var span = MemoryMarshal.Cast<byte, uint>(innerBuffer.Span);
        span[0] = BlockSize;
        span[1] = freeListHead;
        span[2] = RootBlock;
        span[3] = nextBlock;
        await bytes.WriteAsync(innerBuffer, 0);
    }

    public void Dispose() => bytes.Dispose();

    public BlockStreamReader GetReader(uint firstBlock, long streamLength, 
        IEndBlockDataTarget target) => new(this, firstBlock, streamLength, target);

    internal int ReadFromBlockData(
        Span<byte> target, uint block, int offset) =>
        bytes.Read(target.OfMaxLen(DataRemainingInBlock(offset)),
            PositionForDataInBlock(block, offset));

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

    public int WriteToBlockData(ReadOnlySpan<byte> buffer, uint block, int offset)
    {
        var len = (int)Math.Min(DataRemainingInBlock(offset), buffer.Length);
        bytes.Write(buffer.OfMaxLen(len), PositionForDataInBlock(block, offset));
        return len;
    }


    private const int nextBlockTagSize = 4;
    private const long headerSize = 16;

    private long PositionForDataInBlock(uint block, int offset) =>
        (block * BlockSize) + offset + headerSize;

    private long PositionForNextBlockLink(uint block) =>
        PositionForDataInBlock(block + 1, -nextBlockTagSize);

    internal int DataRemainingInBlock(int offset) => (int)BlockDataSize - offset;

    public async Task<uint> NextBlockForAsync(uint currentBlock)
    {
        using var buffer = ArrayPool<byte>.Shared.RentHandle(nextBlockTagSize);
        await bytes.ReadExactAsync(
            buffer.AsMemory(0, nextBlockTagSize), PositionForNextBlockLink(currentBlock));
        var ret = MemoryMarshal.Cast<byte, uint>(buffer)[0];
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

    public async Task<BlockWritingStream> GetWriterAsync(
        IEndBlockDataTarget target) =>
        new BlockWritingStream(this, await NextFreeBlockAsync(), target);

    // if you want both, you must aquire the mutexes in this order to avoid deadlock.
    private readonly SemaphoreSlim freeBlockMutex = new SemaphoreSlim(1);

    public async Task<uint> NextFreeBlockAsync()
    {
        using var _ = await freeBlockMutex.WaitForHandleAsync();
        if (freeListHead is InvalidBlock) return nextBlock++;
        var ret = freeListHead;
        freeListHead = await NextBlockForAsync(freeListHead);
        Debug.Assert(freeListHead != ret);
        return ret;
    }

    public uint NextFreeBlock()
    {
        using var _ = freeBlockMutex.WaitForHandle();
        if (freeListHead is InvalidBlock) return nextBlock++;
        var ret = freeListHead;
        freeListHead = NextBlockFor(freeListHead);
        Debug.Assert(freeListHead != ret);
        return ret;
    }

    public async Task WriteNextBlockLinkAsync(uint currentBlock, uint next)
    {
        Debug.Assert(currentBlock != next);
        using var buffer = ArrayPool<byte>.Shared.RentHandle(nextBlockTagSize);
        MemoryMarshal.Cast<byte, uint>(buffer)[0] = next;
        await bytes.WriteAsync(buffer.AsMemory(0, 4), PositionForNextBlockLink(currentBlock));
    }

    public void WriteNextBlockLink(uint currentBlock, uint next)
    {
        Debug.Assert(currentBlock != next);
        Span<byte> buffer = stackalloc byte[nextBlockTagSize];
        MemoryMarshal.Cast<byte, uint>(buffer)[0] = next;
        bytes.Write(buffer, PositionForNextBlockLink(currentBlock));
    }

    public void Flush() => bytes.Flush();

    public void DeleteStream(StreamEnds stream)
    {
        if (!stream.IsValid()) return;

        AssertNotDeleted(stream);

        chainsPendingDelete.Enqueue(stream);
    }
    [Conditional("DEBUG")]
    public void AssertNotDeleted(StreamEnds stream)
    {
        foreach (var prior in chainsPendingDelete)
        {
            Debug.Assert(stream.Start != prior.Start);
            Debug.Assert(stream.Start != prior.End);
            Debug.Assert(stream.End != prior.End);
            Debug.Assert(stream.Start != prior.End);
        }
    }

    private async Task AddDeletedChainsToFreeList()
    {
        // this method is called inside the free block mutex
        while (chainsPendingDelete.TryDequeue(out var chain))
        {
            await WriteNextBlockLinkAsync(chain.End, freeListHead);
            freeListHead = chain.Start;
        }
    }
}

public record struct StreamEnds(uint Start, uint End)
{
    public static StreamEnds Invalid => new(0xFFFFFFFF, 0xFFFFFFFF);
    public bool IsValid() => Start != 0xFFFFFFFF && End != 0xFFFFFFFF;
}

public record struct StreamDescription(uint Start, uint End, long Length)
{
    public StreamEnds StreamEnds => new(Start, End);

    public static StreamDescription Invalid => new(0xFFFFFFFF, 0xFFFFFFFF, 0);

    public bool Exists() => StreamEnds.IsValid();
}