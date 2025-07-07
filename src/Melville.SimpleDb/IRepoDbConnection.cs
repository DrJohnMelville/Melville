using System;
using System.Data;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Melville.Hacks.Reflection;

namespace Melville.SimpleDb;

public interface IRepoDbConnection
{
    [Obsolete("Sqlite is synchronous so use GetConnection instead")]
    ValueTask<IDbConnection> GetConnectionAsync() => ValueTask.FromResult(GetConnection());
    IDbConnection GetConnection();

    IRepoDbConnection Clone();

    SQLiteBlobWrapper BlobWrapper(string table, string column, long key, bool readOnly);
}

public readonly partial struct SQLiteBlobWrapper(SQLiteBlob blob)
{
    private readonly IntPtr blobHandle = GetHandle(blob);

    public bool IsValid => blobHandle != IntPtr.Zero;

    private static IntPtr GetHandle(SQLiteBlob blob)
    {
        var blobHandle = (CriticalHandle)blob.GetField("_sqlite_blob")!;
        var handle = GetHandle(blobHandle);
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Blob handle is not open or has been disposed.");
        }
        return handle;
    }

    public unsafe void Read(Span<byte> target, int sourceOffset)
    {
        fixed (byte* ptr = target)
        {
            ThrowOnError(sqlite3_blob_read(blobHandle, ptr, target.Length, sourceOffset));
        }
    }

    public unsafe void Write(ReadOnlySpan<byte> data, int sourceOffset)
    {
        fixed (byte* ptr = data)
        {
            ThrowOnError(sqlite3_blob_write(blobHandle, ptr, data.Length, sourceOffset));
        }
    }

    private static void ThrowOnError(SQLiteErrorCode ret)
    {
        if (ret != SQLiteErrorCode.Ok)
            throw new SQLiteException(ret, "Error in custom blob operation");
    }
    #region Imports

    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    static extern void CheckDisposed(SQLiteBlob blob);
    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    static extern void CheckOpen(SQLiteBlob blob);
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_sqlite_blob")]
    static extern ref CriticalHandle GetBlobHandle(SQLiteBlob blob);
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "handle")]
    static extern ref IntPtr GetHandle(CriticalHandle blob);


    [DllImport("SQLite.Interop.dll", EntryPoint = "SI6d0a04aa40b1d0be")]
    static extern unsafe SQLiteErrorCode sqlite3_blob_read(
        IntPtr blob, byte* buffer, int count, int offset);

    [DllImport("SQLite.Interop.dll", EntryPoint = "SI5c335fd80f71405f")]
    static extern unsafe SQLiteErrorCode sqlite3_blob_write(
        IntPtr blob, byte* buffer, int count, int offset);


    #endregion

    public void Reopen(long blockId) => blob.Reopen(blockId);

    public void Dispose() => blob?.Dispose();
}
