using System.Data.SQLite;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Melville.FileSystem.Sqlite;

public class SqliteReadingStream(SqliteFileStore store, long objectId, long blockSize, long size)
    : SqliteBlobStream(blockSize)
{
    /// <inheritdoc />
    public override void Flush()
    {
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        blob?.Dispose();
        blob = null;
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count) =>
        Read(buffer.AsSpan(offset, count));

    /// <inheritdoc />
    public override int Read(Span<byte> buffer)
    {
        int outerReadSize = 0;
        while (InnerReadLength(buffer.Length - outerReadSize) is var innerReadSize and > 0)
        {
            EnsureHasBlob();
            blob.Read(buffer.Slice(outerReadSize, innerReadSize), (int)positionInBlock);
            IncrementPosition(innerReadSize);
            outerReadSize += innerReadSize;
        }

        return outerReadSize;
    }

    /// <inheritdoc />
    public override Task<int> ReadAsync(
        byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
        ReadAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();


    /// <inheritdoc />
    public override async ValueTask<int> ReadAsync(
        Memory<byte> buffer, CancellationToken cancellationToken = new CancellationToken())
    {
        var outerReadSize = 0;
        while (InnerReadLength(buffer.Length - outerReadSize) is var innerReadSize and > 0)
        {
            await EnsureHasBlobAsync();
            blob.Read(buffer.Slice(outerReadSize, innerReadSize).Span, (int)positionInBlock);
            IncrementPosition(innerReadSize);
            outerReadSize += innerReadSize;
        }

        return outerReadSize;
    }

    private int InnerReadLength(int bufferSizeAvailable) =>
        Math.Min(
            (int)Math.Min(BytesLeftToRead(), BytesLeftInBlock()),
            bufferSizeAvailable);

    private long BytesLeftToRead() => Length - Position;

    private long BytesLeftInBlock() => blockSize - positionInBlock;

    /// <inheritdoc />
    public override void SetLength(long value) => throw new NotSupportedException();

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException();

    /// <inheritdoc />
    public override bool CanRead => true;

    /// <inheritdoc />
    public override bool CanSeek => true;

    /// <inheritdoc />
    public override bool CanWrite => false;

    /// <inheritdoc />
    public override long Length { get; } = size;

    /// <inheritdoc />
    protected override void JumpTo(long block, long offset) =>
        (currentBlock, positionInBlock) = (block, offset);

    /// <inheritdoc />
    protected override SQLiteBlob GetNewBlob() =>
        store.GetBlobForReading(objectId, currentBlock, blob);

    /// <inheritdoc />
    protected override Task<SQLiteBlob> GetNewBlobAsync() => 
        store.GetBlobForReadingAsync(objectId, currentBlock, blob);
}