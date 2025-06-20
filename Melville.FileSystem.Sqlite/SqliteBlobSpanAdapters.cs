using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Melville.Hacks.Reflection;

namespace Melville.FileSystem.Sqlite;

public static partial class SqliteBlobSpanAdapters
{
    public static unsafe void Read(this SQLiteBlob blob, Span<byte> target, int sourceOffset)
    {
        CheckDisposed(blob);
        CheckOpen(blob);
        if (sourceOffset < 0) throw new ArgumentException("Source offset must be >= 0");

        var blobHandle = (CriticalHandle)blob.GetField("_sqlite_blob");
        var handle = GetHandle(blobHandle);

        fixed (byte* ptr = target)
        {
            if (handle != IntPtr.Zero)
            {
                var ret = sqlite3_blob_read(handle,
                    ptr, target.Length, sourceOffset);
                if (ret != SQLiteErrorCode.Ok)
                    throw new SQLiteException(ret, "Error in custom read method");
            }
        }
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

}