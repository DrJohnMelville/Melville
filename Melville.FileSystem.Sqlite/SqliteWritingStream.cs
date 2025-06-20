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
        store.UpdateFileData(objectId, Length);
        file.UpdateFileData(Length);
    }

    /// <inheritdoc />
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        CloseCurrentBlob();
        file.UpdateFileData(Length);
        return store.UpdateFileDataAsync(objectId, Length);
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
#warning maybe I can do an optimization by using SetLength to allocate a single blob when I know the final length.
        // if I mess with blocksize, I have to set it in the flush methods;
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
        byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
    WriteAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();

    /// <inheritdoc />
    public override async ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer, 
        CancellationToken cancellationToken = new CancellationToken())
    {
        while (buffer.Length > 0)
        {
            await EnsureHasBlobAsync();
            var destSize = (int)Math.Min(blockSize - positionInBlock, buffer.Length);
            blob.Write(buffer[..destSize].Span, (int)positionInBlock);
            buffer = buffer[destSize..];
            IncrementPosition(destSize);
        }
    }

    private void CloseCurrentBlob()
    {
        blob?.Close();
        blob = null;
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
    protected override SQLiteBlob GetNewBlob()
    {
        return store.GetBlobForWriting(objectId, blockSize, currentBlock, blob);
    }

    #warning  Make this a real async
    /// <inheritdoc />
    protected override Task<SQLiteBlob> GetNewBlobAsync() =>
        store.GetBlobForWritingAsync(objectId, blockSize, currentBlock, blob);
}