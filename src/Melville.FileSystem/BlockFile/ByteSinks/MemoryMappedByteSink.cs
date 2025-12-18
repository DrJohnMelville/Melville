using Melville.Hacks.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.ByteSinks;

public unsafe class MemoryMappedByteSink : IByteSink
{
    private readonly FileStream fs;
    private MemoryMappedFile file;
    private readonly ReaderWriterLockSlim rwLock = new();

    public MemoryMappedByteSink(string path)
    {
        fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        if (fs.Length < 32)
        {
            fs.Write(stackalloc byte[32]);
        }
        CreatePointer();
    }
    [MemberNotNull(nameof(file))]
    private void CreatePointer()
    {
        file = MemoryMappedFile.CreateFromFile(
            fs, null, fs.Length, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, 
            true);
    }

    private unsafe readonly struct FileLocation : IDisposable
    {
        private readonly byte* pointer;
        private readonly MemoryMappedViewAccessor accessor;
        private readonly int length;
        public FileLocation(MemoryMappedFile mmf, long offset, int length)
        {
            this.length = length;
            this.accessor = mmf.CreateViewAccessor(offset, length);
            accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);
            pointer += GetOffset(accessor);
        }

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_offset")]
        extern static ref long GetOffset(UnmanagedMemoryAccessor accessor);

        public void Dispose()
        {
            accessor.SafeMemoryMappedViewHandle.ReleasePointer();
            accessor.Dispose();
        }

        public Span<byte> Span => new(pointer, length);
    }

    public long Length
    {
        get => fs.Length;
        set => throw new NotImplementedException();
    }
    public int Read(Span<byte> target, long offset)
    {
        VerifySize(offset + target.Length);

        rwLock.EnterReadLock();
        var desiredLength = target.Length;
        {
            using var location = new FileLocation(file, offset, desiredLength);
            location.Span.CopyTo(target);
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
        {
            using var location = new FileLocation(file, offset, writeLength);
            source.CopyTo(location.Span);
        }
        rwLock.ExitReadLock();
    }

    private void VerifySize(long newSize)
    {
        if (newSize < Length) return;
        rwLock.EnterWriteLock();
        DisposeMap();
        fs.SetLength(Math.Max(newSize, HintedSize()));
        CreatePointer();
        rwLock.ExitWriteLock();
    }

    long hintedWrites = 0;

    private long HintedSize()
    {
        try {
            return hintedWrites <= 0 ? Length : Length + hintedWrites;
        }
        finally { 
          hintedWrites = 0;
        }
    }

    public void Write(IReadOnlyList<ReadOnlyMemory<byte>> source, long offset)
    {
        foreach (var segment in source)
        {
            Write(segment.Span, offset);
            offset += segment.Length;
            Interlocked.And(ref hintedWrites, -segment.Length);
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

    private void DisposeMap() => file.Dispose();

    public void Flush() { }

    public void HintIntendedWriteSize(long value)
    {
        if (hintedWrites <= 0) hintedWrites = 0;
        Interlocked.Add(ref hintedWrites, value);
    }
}