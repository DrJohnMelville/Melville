using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melville.Linq;

namespace Melville.FileSystem.BlockFile.ByteSinks;

public interface IByteSink: IDisposable
{
    long Length { get; set; }
    int Read(Span<byte> target, long offset);
    long Read(IReadOnlyList<Memory<byte>> targets, long offset);
    ValueTask<int> ReadAsync(Memory<byte> target, long offset);
    ValueTask<long> ReadAsync(IReadOnlyList<Memory<byte>> targets, long offset);
    void Write(ReadOnlySpan<byte> source, long offset);
    void Write(IReadOnlyList<ReadOnlyMemory<byte>> source, long offset);
    ValueTask WriteAsync(ReadOnlyMemory<byte> source, long offset);
    ValueTask WriteAsync(IReadOnlyList<ReadOnlyMemory<byte>> source, long offset);
}

public class MemoryBytesSink: IByteSink
{
    private byte[] data;
    public MemoryBytesSink(byte[]? data = null)
    {
        this.data = data ?? [];
    }
    public long Length
    {
        get => data.Length;
        set => Array.Resize(ref data, (int)value);
    }

    public void Dispose() { }
    public int Read(Span<byte> target, long offset)
    {
        var ret = (int)Math.Min(Length - offset, target.Length);
        data.AsSpan((int)offset, ret).CopyTo(target);
        return ret;
    }

    public long Read(IReadOnlyList<Memory<byte>> targets, long offset)
    {
        var total = 0L;
        foreach (var target in targets)
        {
            var local = Read(target.Span, offset + total);
            total += local;
            if (local < target.Length) break;
        }
        return total;
    }
    public ValueTask<int> ReadAsync(Memory<byte> target, long offset) => new(Read(target.Span, offset));
    public ValueTask<long> ReadAsync(IReadOnlyList<Memory<byte>> targets, long offset) => new(Read(targets, offset));
    public void Write(ReadOnlySpan<byte> source, long offset)
    {
        var nextPos = offset + source.Length;
        if (nextPos > Length) Length = nextPos;
        source.CopyTo(data.AsSpan((int)offset));
    }

    public void Write(IReadOnlyList<ReadOnlyMemory<byte>> source, long offset)
    {
        foreach (var item in source)
        {
            Write(item.Span, offset);
            offset += item.Length;
        }
    }
    public ValueTask WriteAsync(ReadOnlyMemory<byte> source, long offset)
    {
        Write(source.Span, offset);
        return ValueTask.CompletedTask;
    }
    public ValueTask WriteAsync(IReadOnlyList<ReadOnlyMemory<byte>> source, long offset)
    {
        Write(source, offset);
        return ValueTask.CompletedTask;
    }

    public string Dump => data.BinaryDump();

}