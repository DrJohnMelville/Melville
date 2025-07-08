using System.Text.RegularExpressions;

namespace Melville.FileSystem.Sqlite;

public abstract partial class SqliteFileSystemObject(
    SqliteFileStore store, string name, string path, long parentId) : IFileSystemObject
{
    protected readonly SqliteFileStore store = store;
    protected long parentDirectoryId = parentId;
    protected long objectId;

    /// <inheritdoc />
    public string Path { get; } = path;

    /// <inheritdoc />
    public IDirectory? Directory
    {
        get
        {
            if (parentDirectoryId == 0) return null;
            var parentFso = store.DirectoryById(parentDirectoryId);
            if (parentFso is not { BlockSize: < 0 }) return null;
                return new SqliteDirectory(store, parentFso, RemoveTwoLevels(Path));
        }
    }

    private string RemoveTwoLevels(string s) => TwoLeafRemover().Replace(s,"");

    [GeneratedRegex(@"([/\\]?[^/\\]*){2}$")]
    private static partial Regex TwoLeafRemover();

    /// <inheritdoc />
    public string Name { get; } = name;

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
        if (!Exists()) return;
        store.DeleteItem(objectId);
        objectId = 0;
        parentDirectoryId = 0;
        Attributes = FileAttributes.None;
        LastWrite = Created = new DateTime();
    }

    public virtual void PopulateFrom(FSObject source)
    {
        objectId = source.Id;
        parentDirectoryId = source.Parent ?? 0;
        Created = new DateTime(source.CreatedTime);
        LastWrite = new DateTime(source.LastWrite);
        Attributes = (FileAttributes)source.Attributes;
    }

    protected void MustExist()
    {
        if (!Exists())
            throw new InvalidOperationException("A directory must exist to do this operation");
    }
}