using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
    void Flush();
}

public static class ByteSinkOperations
{
    public static async Task ReadExactAsync(
        this IByteSink sink, Memory<byte> target, long offset, int tries = 3)
    {
        for (int i = 0; i < tries; i++)
        {
            if (await sink.ReadAsync(target, offset) == target.Length)
                return;
        }
        throw new IOException(
            $"Failed to read {target.Length} bytes from sink at offset {offset} after {tries} tries.");
    }

    public static void ReadExact(
        this IByteSink sink, Span<byte> target, long offset, int tries = 3)
    {
        for (int i = 0; i < tries; i++)
        {
            if (sink.Read(target, offset) == target.Length)
                return;
        }
        throw new IOException(
            $"Failed to read {target.Length} bytes from sink at offset {offset} after {tries} tries.");
    }
}