using Melville.FileSystem.BlockFile.ByteSinks;
using Melville.Hacks;
using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public class ReadOnlyBlockMultiStream(IByteSink bytes, uint blockSize = 4096 + 4, uint rootBlock = ReadOnlyBlockMultiStream.InvalidBlock): IDisposable
{
    protected readonly IByteSink bytes = bytes;
    protected const int nextBlockTagSize = 4;
    protected const long headerSize = 16;
    public const uint InvalidBlock = 0xFFFFFFFF;

    public long BlockSize { get; } = blockSize;
    public uint RootBlock { get; protected set; } = rootBlock;
    public uint BlockDataSize => blockSize - nextBlockTagSize;
    public void Dispose() => bytes.Dispose();

    public static async Task<ReadOnlyBlockMultiStream> CreateReadOnlyFrom(IByteSink bytes)
    {
        if (bytes.Length < 16)
            return new BlockMultiStream(bytes);
        using var buffer = ArrayPool<byte>.Shared.RentHandle(16);
        var bufferMem = buffer.AsMemory(0, 16);
        await bytes.ReadExactAsync(bufferMem, 0);
        var sizes = MemoryMarshal.Cast<byte, uint>(bufferMem.Span);
        if (sizes[0] < 1) sizes[0] = 4096;
        return new ReadOnlyBlockMultiStream(bytes, sizes[0], sizes[2]);
    }
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

    protected long PositionForDataInBlock(uint block, int offset) =>
    (block * BlockSize) + offset + headerSize;

    protected long PositionForNextBlockLink(uint block) =>
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
}
