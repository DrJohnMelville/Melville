using System;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public static class MemoryOperations
{
    public static Memory<T> OfMaxLen<T>(this Memory<T> memory, long length) => 
        length < memory.Length ? memory.Slice(0, (int)length) : memory;

    public static ReadOnlyMemory<T> OfMaxLen<T>(this ReadOnlyMemory<T> memory, long length) => 
        length < memory.Length ? memory.Slice(0, (int)length) : memory;
}