using Melville.Hacks.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.ByteSinks;

public unsafe class MemoryMappedByteSink: IByteSink
{
    private readonly FileStream fs;
    private MemoryMappedFile file;
    private MemoryMappedViewAccessor accessor;
    private byte* basePointer;
    private readonly ReaderWriterLockSlim rwLock = new();
    
    public MemoryMappedByteSink(string path)
    {
        fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        CreatePointer();
    }
    [MemberNotNull(nameof(file))]
    [MemberNotNull(nameof(accessor))]
    private void CreatePointer()
    {
        file = MemoryMappedFile.CreateFromFile(
            fs, null, fs.Length, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, true);
        accessor = file.CreateViewAccessor(0, 0, MemoryMappedFileAccess.ReadWrite);
        accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref basePointer);
    }

    private Span<Byte> GetSpan(long offset, int length) =>
        new Span<byte>(basePointer + offset, length);

    public long Length { 
        get => fs.Length; 
        set => throw new NotImplementedException(); 
    }
    public int Read(Span<byte> target, long offset)
    {
        VerifySize(offset + target.Length);

        rwLock.EnterReadLock();
        var desiredLength = target.Length;
        {
            GetSpan(offset, desiredLength).CopyTo(target);
        }

        rwLock.ExitReadLock();
        return desiredLength;
    }

    public long Read(IReadOnlyList<Memory<byte>> targets, long offset)
    {
        var read = 0L;
        foreach (var target in targets)
        {
            read += Read(target.Span, offset);
            offset += target.Length;
        }   
        return read;
    }
    public ValueTask<int> ReadAsync(Memory<byte> target, long offset) => 
        new ValueTask<int>(Read(target.Span, offset));
    public ValueTask<long> ReadAsync(IReadOnlyList<Memory<byte>> targets, long offset) =>
        new ValueTask<long>(Read(targets, offset));
    public void Write(ReadOnlySpan<byte> source, long offset)
    {
        int writeLength = source.Length;
        VerifySize(offset + writeLength);
        rwLock.EnterReadLock();
            source.CopyTo(GetSpan(offset, writeLength));
        rwLock.ExitReadLock();
    }

    private void VerifySize(long newSize)
    {
        if (newSize < Length) return;
        rwLock.EnterWriteLock();
        DisposeMap();
        fs.SetLength(newSize);
        CreatePointer();
        rwLock.ExitWriteLock();
    }

    public void Write(IReadOnlyList<ReadOnlyMemory<byte>> source, long offset)
    {
        foreach (var segment in source)
        {
            Write(segment.Span, offset);
            offset += segment.Length;
        }
    }
    public ValueTask WriteAsync(ReadOnlyMemory<byte> source, long offset)
    {
        Write(source.Span, offset);
        return ValueTask.CompletedTask;
    }
    public ValueTask WriteAsync(
        IReadOnlyList<ReadOnlyMemory<byte>> source, long offset)
    {
        Write(source, offset);
        return ValueTask.CompletedTask;
    }
    public void Dispose()
    {
        DisposeMap();
        fs.Dispose();
    }

    private void DisposeMap()
    {
        accessor.Dispose();
        file.Dispose();
    }

    public void Flush() { }
}
