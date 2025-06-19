namespace Melville.FileSystem.Sqlite;

public class SqliteDirectory: SqliteFileSystemObject, IDirectory
{
    /// <inheritdoc />
    public IDirectory SubDirectory(string name)
    {
        return null;
    }

    /// <inheritdoc />
    public IFile File(string name)
    {
        return null;
    }

    /// <inheritdoc />
    public IEnumerable<IFile> AllFiles()
    {
        yield break;
    }

    /// <inheritdoc />
    public IEnumerable<IFile> AllFiles(string glob)
    {
        yield break;
    }

    /// <inheritdoc />
    public IEnumerable<IDirectory> AllSubDirectories()
    {
        yield break;
    }

    /// <inheritdoc />
    public void Create(FileAttributes attributes = FileAttributes.Directory)
    {
    }

    /// <inheritdoc />
    public IFile FileFromRawPath(string path)
    {
        return null;
    }

    /// <inheritdoc />
    public bool IsVolitleDirectory()
    {
        return false;
    }

    /// <inheritdoc />
    public IDisposable? WriteToken()
    {
        return null;
    }
}