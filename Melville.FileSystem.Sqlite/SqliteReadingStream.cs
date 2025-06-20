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
    public override int Read(byte[] buffer, int offset, int count)
    {
        return Read(buffer.AsSpan(offset, count));
    }

    /// <inheritdoc />
    public override int Read(Span<byte> buffer)
    {
#warning we should reuse our blob objects rather than opening and closing them
        int outerBytesRead = 0;
        while (buffer.Length > 0 && Length-Position is var bytesLeftInStream and > 0)
        {
            EnsureHasBlob();
            var readLen = Math.Min((int)Math.Min(bytesLeftInStream, BytesLeftInBlock()),
                buffer.Length);
            blob.Read(buffer[..readLen], (int)positionInBlock);
            IncrementPosition(readLen);
            outerBytesRead += readLen;
            buffer = buffer[readLen..];
        }
        return outerBytesRead;
    }

    private long BytesLeftInBlock()
    {
        return blockSize - positionInBlock;
    }

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
    protected override void JumpTo(long block, long offset)
    {
        currentBlock = block;
        positionInBlock = offset;
    }

    /// <inheritdoc />
    protected override SQLiteBlob GetNewBlob()
    {
        blob?.Dispose();
        
        return store.GetBlobForReading(objectId, (int)currentBlock);
    }
}