using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace Melville.FileSystem.BlockFile.ByteSinks;

public class FileByteSink(string path,
    FileMode mode = FileMode.OpenOrCreate, 
    FileAccess access = FileAccess.ReadWrite, 
    FileShare share = FileShare.None, 
    FileOptions options = FileOptions.Asynchronous | FileOptions.RandomAccess, 
    long preallocation = 0) : IByteSink
{
    private readonly SafeFileHandle handle = File.OpenHandle(path, mode, access, share, options, preallocation);

    public long Length
    {
        get => RandomAccess.GetLength(handle);
        set => RandomAccess.SetLength(handle, value);
    }
    public int Read(Span<byte> target, long offset) => 
        RandomAccess.Read(handle, target, offset);
    public long Read(IReadOnlyList<Memory<byte>> targets, long offset) =>
        RandomAccess.Read(handle, targets, offset);
    public ValueTask<int> ReadAsync(Memory<byte> target, long offset) =>
        RandomAccess.ReadAsync(handle, target, offset);
    public ValueTask<long> ReadAsync(IReadOnlyList<Memory<byte>> targets, long offset) =>
        RandomAccess.ReadAsync(handle, targets, offset);
    public void Write(ReadOnlySpan<byte> source, long offset) =>
        RandomAccess.Write(handle, source, offset);
    public void Write(IReadOnlyList<ReadOnlyMemory<byte>> source, long offset) =>
        RandomAccess.Write(handle, source, offset);
    public ValueTask WriteAsync(ReadOnlyMemory<byte> source, long offset) =>
        RandomAccess.WriteAsync(handle, source, offset);
    public ValueTask WriteAsync(IReadOnlyList<ReadOnlyMemory<byte>> source, long offset) =>
        RandomAccess.WriteAsync(handle, source, offset);

    public void Dispose() => handle.Dispose();

    /// <inheritdoc />
    public void Flush() => RandomAccess.FlushToDisk(handle);
}