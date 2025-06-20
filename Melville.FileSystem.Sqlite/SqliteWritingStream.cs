using System.Data.SQLite;
using System.Security.Cryptography;

namespace Melville.FileSystem.Sqlite;

public class SqliteWritingStream(SqliteFileStore store, long objectId, long blockSize,
    SqliteFile file) : Stream
{
    private int currentBlock;
    private int positionInBlock;
    private SQLiteBlob? currentBlob;
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
    public override long Seek(long offset, SeekOrigin origin) => 
        throw new NotSupportedException();

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
#warning right now I am doing something stupid performance wise, because I think that later I can write spans to the blob
        while (buffer.Length > 0)
        {
            EnsureHasBlob();
            var destSize = (int)Math.Min(blockSize - positionInBlock, buffer.Length);
            var data = buffer.Slice(0, destSize).ToArray();
            currentBlob.Write(data, destSize, positionInBlock);
            buffer = buffer[destSize..];
            IncrementalPosition(destSize);
        }
    }

    private void IncrementalPosition(int destSize)
    {
        positionInBlock += destSize;
        if (positionInBlock >= blockSize)
        {
            CloseCurrentBlob();
            positionInBlock = 0;
            currentBlock++;
        }
    }

    private void CloseCurrentBlob()
    {
        currentBlob?.Close();
        currentBlob = null;
    }

    private void EnsureHasBlob()
    {
        if (currentBlob is not null) return;
        currentBlob = store.GetBlobForWriting(objectId, blockSize, currentBlock);
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
    public override long Position
    {
        get => (blockSize*currentBlock)+positionInBlock;
        set => throw new NotSupportedException("Cannot seek a SqliteWritingStream");
    }
}