namespace Melville.FileSystem.Sqlite;

public sealed class SqliteFile(SqliteFileStore store, string _name, string _path, long parentId) :
    SqliteFileSystemObject(store, _name, _path, parentId), IFile
{
    public SqliteFile(SqliteFileStore sqliteFileStore, FSObject dto, string path) :
        this(sqliteFileStore, dto.Name, $"{path}/{dto.Name}", dto.Parent ?? 0)
    {
        PopulateFrom(dto);
    }

    /// <inheritdoc />
    public async Task<Stream> OpenRead()
    {
        if (!Exists())
            throw new IOException("File does not exist.");
        return new MemoryStream();
    }

    /// <inheritdoc />
    public async Task<Stream> CreateWrite(FileAttributes attributes = FileAttributes.Normal)
    {
        if (!Exists())
        {
            var dto = await store.CreateItemAsync(Name, parentDirectoryId,
                attributes & ~FileAttributes.Directory, 4096);
            PopulateFrom(dto);
        }
        return new MemoryStream();
    }

    /// <inheritdoc />
    public byte FinalProgress => 0;

    /// <inheritdoc />
    public Task WaitForFinal => Task.CompletedTask;

    /// <inheritdoc />
    public long Size { get; private set; }

    /// <inheritdoc />
    public override void PopulateFrom(FSObject source)
    {
        Size = source.Length;
        if (source.BlockSize <= 0)
            throw new InvalidOperationException("Block size must be positive files.");
        base.PopulateFrom(source);
    }
}