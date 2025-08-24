using System;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public static class MemoryOperations
{
    public static Memory<T> OfMaxLen<T>(this Memory<T> memory, long length) => 
        length < memory.Length ? memory.Slice(0, (int)length) : memory;

    public static ReadOnlyMemory<T> OfMaxLen<T>(this ReadOnlyMemory<T> memory, long length) => 
        length < memory.Length ? memory.Slice(0, (int)length) : memory;

    public static Span<T> OfMaxLen<T>(this Span<T> span, long length) => 
        length < span.Length ? span.Slice(0, (int)length) : span;

    public static ReadOnlySpan<T> OfMaxLen<T>(this ReadOnlySpan<T> span, long length) => 
        length < span.Length ? span.Slice(0, (int)length) : span;
}