using System;
using System.IO;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public abstract class BlockFileSystemObject(BlockDirectory? parent, string name): IFileSystemObject
{
    protected virtual BlockDirectory? Parent { get; } = parent;

    /// <inheritdoc />
    public virtual string Path => $"{Parent?.Path}/{Name}";

    /// <inheritdoc />
    public IDirectory? Directory => Parent;

    /// <inheritdoc />
    public string Name { get; } = name;

    /// <inheritdoc />
    public abstract bool Exists();

    /// <inheritdoc />
    public bool ValidFileSystemPath() => false;

    /// <inheritdoc />
    public DateTime LastAccess => default;

    /// <inheritdoc />
    public DateTime LastWrite => default;

    /// <inheritdoc />
    public DateTime Created => default;

    public abstract void Delete();
    public abstract FileAttributes Attributes { get; }
}