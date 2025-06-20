using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Melville.Hacks.Reflection;

namespace Melville.FileSystem.Sqlite;

public static partial class SqliteBlobSpanAdapters
{
    public static unsafe void Read(this SQLiteBlob blob, Span<byte> target, int sourceOffset)
    {
        var handle = GetHandle(blob, sourceOffset);
        fixed (byte* ptr = target)
        {
                ThrowOnError(sqlite3_blob_read(handle, ptr, target.Length, sourceOffset));
        }
    }

    private static IntPtr GetHandle(SQLiteBlob blob, int sourceOffset)
    {
        CheckDisposed(blob);
        CheckOpen(blob);
        if (sourceOffset < 0) throw new ArgumentException("Source offset must be >= 0");
        var blobHandle = (CriticalHandle)blob.GetField("_sqlite_blob");
        var handle = GetHandle(blobHandle);
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Blob handle is not open or has been disposed.");
        }
        return handle;
    }

    public static unsafe void Write(this SQLiteBlob blob, ReadOnlySpan<byte> data, int sourceOffset)
    {
        var handle = GetHandle(blob, sourceOffset);
        fixed (byte* ptr = data)
        {
                ThrowOnError(sqlite3_blob_write(handle, ptr, data.Length, sourceOffset));
        }
    }

    private static void ThrowOnError(SQLiteErrorCode ret)
    {
        if (ret != SQLiteErrorCode.Ok)
            throw new SQLiteException(ret, "Error in custom blob operation");
    }

    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    static extern void CheckDisposed(SQLiteBlob blob);
    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    static extern void CheckOpen(SQLiteBlob blob);
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_sqlite_blob")]
    static extern ref CriticalHandle GetBlobHandle(SQLiteBlob blob);
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "handle")]
    static extern ref IntPtr GetHandle(CriticalHandle blob);


    [DllImport("SQLite.Interop.dll", EntryPoint = "SI6d0a04aa40b1d0be", 
        CallingConvention = CallingConvention.Cdecl)]
    static extern unsafe SQLiteErrorCode sqlite3_blob_read(
        IntPtr blob,
        byte* buffer,
        int count,
        int offset);

    [DllImport("SQLite.Interop.dll", EntryPoint = "SI5c335fd80f71405f", CallingConvention = CallingConvention.Cdecl)]
    static extern unsafe SQLiteErrorCode sqlite3_blob_write(
        IntPtr blob,
        byte* buffer,
        int count,
        int offset);


}