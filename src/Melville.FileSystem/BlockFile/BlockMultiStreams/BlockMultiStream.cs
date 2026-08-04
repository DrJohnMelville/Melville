using Melville.FileSystem.BlockFile.ByteSinks;
using Melville.FileSystem.BlockFile.FileSystemObjects;
using Melville.Hacks;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public class BlockMultiStream(
    IByteSink bytes,
    uint blockSize = 4096 + 4, // defaults to 4k of data + 4 bytes for the next block tag
    uint freeListHead = BlockMultiStream.InvalidBlock,
    uint rootBlock = BlockMultiStream.InvalidBlock,
    uint nextBlock = 0) : ReadOnlyBlockMultiStream(bytes, blockSize, rootBlock)
{

    // blocksize is a long to force all math it is involved with to be a long
    private uint nextBlock = nextBlock;
    private uint freeListHead = freeListHead;

    private readonly ConcurrentQueue<StreamEnds>
        chainsPendingDelete = new();

    public static async Task<BlockMultiStream> CreateFrom(IByteSink bytes)
    {
        if (bytes.Length < 16)
            return new BlockMultiStream(bytes);
        using var buffer = ArrayPool<byte>.Shared.RentHandle(16);
        var bufferMem = buffer.AsMemory(0, 16);
        await bytes.ReadExactAsync(bufferMem, 0);
        var sizes = MemoryMarshal.Cast<byte, uint>(bufferMem.Span);
        VerifySizeAndBlocks(sizes);
        return new BlockMultiStream(bytes, sizes[0], sizes[1], sizes[2], sizes[3]);
    }

    private static void VerifySizeAndBlocks(Span<uint> sizes)
    {
        ProductionAssert(sizes[0] > 0);
        Debug.Assert(sizes[0] == 4096); //technicall supports arbitrary block sizes, but we don't use them yet. so use the extra verification in debug builds
        ProductionAssert(sizes[2] != sizes[3]);
        ProductionAssert(sizes[1] != sizes[3]);
        ProductionAssert(sizes[1] != sizes[2]);
    }

    private static void ProductionAssert(bool condition, [CallerArgumentExpression(nameof(condition))] string? message = null)
    {
        if (!condition)
            throw new InvalidOperationException($"Failed assertion in BlockMultiStream: {message}");
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
        span[0] = (uint)BlockSize;
        span[1] = freeListHead;
        span[2] = RootBlock;
        span[3] = nextBlock;
        VerifySizeAndBlocks(span);
        await bytes.WriteAsync(innerBuffer, 0);
    }

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
        var span = buffer.AsSpan(0, nextBlockTagSize);
        MemoryMarshal.Cast<byte, uint>(span)[0] = next;
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

    internal void HintIntendedWriteSize(long value) => 
        bytes.HintIntendedWriteSize(ConvertToBlocks(value));

    private long ConvertToBlocks(long value)
    {
        var blocks = (value + BlockDataSize - 1) / BlockDataSize;
        return blocks * BlockSize;
    }
}
