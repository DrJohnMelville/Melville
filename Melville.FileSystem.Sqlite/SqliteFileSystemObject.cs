namespace Melville.FileSystem.Sqlite;

public abstract class SqliteFileSystemObject : IFileSystemObject
{
    private long parentDirectoryId;
    private long objectId;
    /// <inheritdoc />
    public string Path { get; private set; }

    /// <inheritdoc />
    public IDirectory? Directory => null;

    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public bool Exists() => objectId > 0;

    /// <inheritdoc />
    public bool ValidFileSystemPath() => false;

    /// <inheritdoc />
    public DateTime LastAccess => new DateTime();

    /// <inheritdoc />
    public DateTime LastWrite { get; private set; }

    /// <inheritdoc />
    public DateTime Created { get; private set; }

    /// <inheritdoc />
    public FileAttributes Attributes { get; private set; }

    /// <inheritdoc />
    public void Delete()
    {
    }
}