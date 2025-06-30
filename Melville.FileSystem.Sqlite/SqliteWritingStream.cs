using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;


namespace Melville.FileSystem.Sqlite;

public class SqliteWritingStream(SqliteFileStore store, long objectId, long blockSize,
    SqliteFile file) : SqliteBlobStream(blockSize)
{
    /// <inheritdoc />
    public override void Flush()
    {
        CloseCurrentBlob();
        store.UpdateFileData(objectId, Length, blockSize);
        file.UpdateFileData(Length, blockSize);
    }

    /// <inheritdoc />
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        Flush();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing) => Flush();

    /// <inheritdoc />
    public override ValueTask DisposeAsync() => new(FlushAsync());

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();


    /// <inheritdoc />
    public override void SetLength(long value)
    {
       if (Position == 0)
            blockSize = value > 0 ? value : 4096;
    }

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count) => 
        Write(buffer.AsSpan(offset, count));

    /// <inheritdoc />
    public override void Write(ReadOnlySpan<byte> buffer)
    {
        while (buffer.Length > 0)
        {
            EnsureHasBlob();
            var destSize = (int)Math.Min(blockSize - positionInBlock, buffer.Length);
            blob.Write(buffer[..destSize], (int)positionInBlock);
            buffer = buffer[destSize..];
            IncrementPosition(destSize);
        }
    }

    /// <inheritdoc />
    public override Task WriteAsync(
        byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        Write(buffer, offset, count);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer, 
        CancellationToken cancellationToken = new CancellationToken())
    {
        Write(buffer.Span);
        return ValueTask.CompletedTask;
    }

    private void CloseCurrentBlob()
    {
        blob.Dispose();
        blob = default;
    }

    /// <inheritdoc />
    public override bool CanRead => false;

    /// <inheritdoc />
    public override bool CanSeek => false;

    /// <inheritdoc />
    public override bool CanWrite => true;

    /// <inheritdoc />
    public override long Length => Position;

    /// <inheritdoc />
    protected override void JumpTo(long block, long offset) => throw new NotSupportedException();

    /// <inheritdoc />
    protected override SQLiteBlobWrapper GetNewBlob() => 
        store.GetBlobForWriting(objectId, blockSize, currentBlock, blob);
}