using System.Text.RegularExpressions;
using Melville.FileSystem.PseudoTransactedFS;

namespace Melville.FileSystem.Sqlite;

public sealed class SqliteTransactedDirectory(
    SqliteFileStore store, string _name, string _path, long parentId) :
    SqliteDirectory(store, _name, _path, parentId), ITransactedDirectory
{
    public ValueTask Commit()
    {
        store.Commit();
        return ValueTask.CompletedTask;
    }

    public void Rollback() => store.Rollback();

    public void Dispose() => store.DisposeTransaction();
}

public class SqliteDirectory(SqliteFileStore store, string _name, string _path, long parentId):
    SqliteFileSystemObject(store,_name,_path, parentId), IDirectory
{
    public SqliteDirectory(SqliteFileStore sqliteFileStore, FSObject dto, string path) : 
        this(sqliteFileStore, dto.Name, $"{path}/{dto.Name}", dto.Parent ?? 0)
    {
        PopulateFrom(dto);
    }

    /// <inheritdoc />
    public IDirectory SubDirectory(string name)
    {
        MustExist();
        var ret = new SqliteDirectory(store, name, $"{Path}\\{name}", objectId);
        return store.TryCreateObject(ret, objectId, true);
    }

    /// <inheritdoc />
    public IFile File(string name)
    {
        MustExist();
        var ret = new SqliteFile(store, name, $"{Path}\\{name}", objectId);
        return store.TryCreateObject(ret, objectId, false);
    }

    /// <inheritdoc />
    public IEnumerable<IFile> AllFiles() => 
        store.Files(objectId).Select(CreateFileFromFsObject);

    private SqliteFile CreateFileFromFsObject(FSObject i) => new(store, i, Path);

    /// <inheritdoc />
    public IEnumerable<IFile> AllFiles(string glob)
    {
        var regex = new Regex($"^{RegexExtensions.GlobToRegex(glob)}$");
        return store.Files(objectId)
            .Where(i => regex.IsMatch(i.Name))
            .Select(CreateFileFromFsObject);
    }

    /// <inheritdoc />
    public IEnumerable<IDirectory> AllSubDirectories()
    {
        MustExist();
        return store.SubDirectories(objectId).Select(i => new SqliteDirectory(
            store, i, Path));
    }

    /// <inheritdoc />
    public void Create(FileAttributes attributes = FileAttributes.Directory)
    {
        if (Exists()) return;
        PopulateFrom(store.CreateItem(Name, parentDirectoryId, attributes |FileAttributes.Directory, -1));
    }

    /// <inheritdoc />
    public IFile FileFromRawPath(string path)
    {
        throw new NotImplementedException("Sqlite provider does not support raw paths");
    }

    /// <inheritdoc />
    public bool IsVolitleDirectory() => false;

    /// <inheritdoc />
    public IDisposable? WriteToken() => new FakeDisposable();

    private class FakeDisposable() : IDisposable
    {
        //In the Sqlite provider the file handle to the Sqlite database is held open for
        // the entire time the folder is held, so a  separate write token is not needed.
        // This fake write token makes the UI happy by giving it a token to hold onto and delete
        // at the proper moment, but it does not actually do anything.

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }

    /// <inheritdoc />
    public override void PopulateFrom(FSObject source)
    {
        if (source.BlockSize >= 0)
            throw new InvalidDataException("A directory must have a negative blocksize.");
        base.PopulateFrom(source);
    }
}