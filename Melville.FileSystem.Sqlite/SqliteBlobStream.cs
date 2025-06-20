﻿using System.Data.SQLite;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Melville.FileSystem.Sqlite;

public abstract class SqliteBlobStream(long blockSize) : Stream
{
    protected long currentBlock;
    protected long positionInBlock;
    protected SQLiteBlob? blob;
    protected long blobIsForBlock = -1;
    protected long blockSize = blockSize;

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
    public override long Position
    {
        get => (currentBlock * blockSize) + positionInBlock;
        set
        {
            JumpTo(value/blockSize, value%blockSize);
        }
    }

    protected abstract void JumpTo(long block, long offset);

    protected void IncrementPosition(int delta)
    {
        positionInBlock += delta;
        Debug.Assert(positionInBlock <= blockSize);
        if (positionInBlock == blockSize)
        {
            currentBlock++;
            positionInBlock = 0;
        }
    }

    [MemberNotNull(nameof(blob))]
    public void EnsureHasBlob()
    {
        if (blob is not null && blobIsForBlock == currentBlock) return;
        blob?.Dispose();
        blob = GetNewBlob();
        blobIsForBlock = currentBlock;
    }

    protected abstract SQLiteBlob GetNewBlob();
}