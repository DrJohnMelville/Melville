using System.Data.SQLite;

namespace Melville.FileSystem.Sqlite;

public class SqliteReadingStream(SqliteFileStore store, long objectId, long blockSize, long size) : Stream
{
    private long blockIndex = 0;
    private long blockPosition = 0;
    private SQLiteBlob? blob;

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
            blob.Read(buffer[..readLen], (int)blockPosition);
            IncrementPosition(readLen);
            outerBytesRead += readLen;
            buffer = buffer[readLen..];
        }
        return outerBytesRead;
    }

    private long BytesLeftInBlock()
    {
        return blockSize - blockPosition;
    }

    private void IncrementPosition(int readLen)
    {
        blockPosition += readLen;
        if (blockPosition >= blockSize)
        {
            blockIndex++;
            blockPosition = 0;
            blob?.Dispose();
            blob = null;
        }
    }

    private void EnsureHasBlob()
    {
        if (blob is not null || Position >= Length) return;
        blob = store.GetBlobForReading(objectId, blockIndex);
    }


    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                Position = offset;
                break;
            case SeekOrigin.Current:
                Position += offset;
                break;
            case SeekOrigin.End:
                Position = Length + offset;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
        }
        return Position;
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
    public override long Position
    {
        get => (blockIndex*blockSize)+blockPosition;
        set
        {
            if ((ulong)value >= (ulong)Length)
                throw new ArgumentOutOfRangeException(nameof(value), "Position is beyond the end of the stream.");
            blockIndex = value / blockSize;
            blockPosition = value % blockSize;
        }
    }

    private void SetBlockIndex(int newIndex)
    {
        if (blockIndex == newIndex) return;
        blob?.Dispose();
        blob = null;
    }
}