using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Melville.SimpleDb;

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
        blob.Dispose();
        blob = default;
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
            blob.Read(buffer.Slice(outerReadSize, innerReadSize));
            IncrementPosition(innerReadSize);
            outerReadSize += innerReadSize;
        }

        return outerReadSize;
    }

    /// <inheritdoc />
    public override Task<int> ReadAsync(
        byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
        Task.FromResult(Read(buffer.AsSpan(offset, count)));


    /// <inheritdoc />
    public override  ValueTask<int> ReadAsync(
        Memory<byte> buffer, CancellationToken cancellationToken = new CancellationToken()) =>
        new(Read(buffer.Span));

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
    protected override Stream GetNewBlob() =>
        store.GetBlobForReading(objectId, currentBlock);
}