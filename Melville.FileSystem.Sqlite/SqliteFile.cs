using System.Data.SQLite;

namespace Melville.FileSystem.Sqlite;

public sealed class SqliteFile(SqliteFileStore store, string _name, string _path, long parentId) :
    SqliteFileSystemObject(store, _name, _path, parentId), IFile
{
    private long blockSize = 4096;
    public SqliteFile(SqliteFileStore sqliteFileStore, FSObject dto, string path) :
        this(sqliteFileStore, dto.Name, $"{path}/{dto.Name}", dto.Parent ?? 0)
    {
        PopulateFrom(dto);
    }

    /// <inheritdoc />
    public Task<Stream> OpenRead()
    {
        MustExist();
        return Task.FromResult<Stream>(
            new SqliteReadingStream(store, objectId, blockSize, Size));
    }

    /// <inheritdoc />
    public Task<Stream> CreateWrite(FileAttributes attributes = FileAttributes.Normal)
    {
        if (!Exists())
        {
            var dto = store.CreateItem(Name, parentDirectoryId,
                attributes & ~FileAttributes.Directory, blockSize);
            PopulateFrom(dto);
        }
        else
        {
            store.DeleteBlocksFor(objectId);
        }
        return Task.FromResult<Stream>(new SqliteWritingStream(store, objectId, blockSize, this));
    }

    /// <inheritdoc />
    public byte FinalProgress => 255;

    /// <inheritdoc />
    public Task WaitForFinal => Task.CompletedTask;

    /// <inheritdoc />
    public long Size { get; private set; }

    /// <inheritdoc />
    public override void PopulateFrom(FSObject source)
    {
        Size = source.Length;
        if (source.BlockSize <= 0)
            throw new InvalidOperationException("Block size must be positive for files.");
        blockSize = source.BlockSize;
        Size = source.Length;
        base.PopulateFrom(source);
    }

    internal void UpdateFileData(long length) => Size = length;
}