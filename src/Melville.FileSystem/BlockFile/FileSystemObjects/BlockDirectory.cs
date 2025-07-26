using Melville.FileSystem.BlockFile.BlockMultiStreams;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public class BlockRootDirectory(BlockMultiStream store) : 
    BlockDirectory(null, "")
{
    public override string Path => "";

    public override BlockMultiStream Store => store;
}

public class BlockDirectory(BlockDirectory? parent, string name): 
    BlockFileSystemObject(parent, name), IDirectory
{
    public virtual BlockMultiStream Store => 
        parent?.Store ?? throw new ArgumentNullException(nameof(parent));

    private SortedDictionary<string, BlockDirectory> directories = new();
    private SortedDictionary<string, IFile> files = new();

    public IDirectory SubDirectory(string name)
    {
        if (directories.TryGetValue(name, out var result))
            return result;
        var newDir = new BlockDirectory(this, name);
        directories.Add(name, newDir);
        return newDir;
    }

    /// <inheritdoc />
    public IFile File(string name)
    {
        if (files.TryGetValue(name, out var result))
            return result;
        var newFile = new BlockFile(this, name);
        files.Add(name, newFile);
        return newFile;
    }

    /// <inheritdoc />
    public IEnumerable<IFile> AllFiles()
    {
        return files.Values
            .Where(i=>i.Exists());
    }

    /// <inheritdoc />
    public IEnumerable<IFile> AllFiles(string glob)
    {
        var regex = new Regex($"^{RegexExtensions.GlobToRegex(glob)}$");
        return AllFiles().Where(i => regex.IsMatch(i.Name));
    }

    /// <inheritdoc />
    public IEnumerable<IDirectory> AllSubDirectories()
    {
        return directories.Values;
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
    public IDisposable? WriteToken() => new FakeToken();

    private class FakeToken: IDisposable
    {
        public void Dispose(){}
    }

    /// <inheritdoc />
    public override bool Exists() => true;

    /// <inheritdoc />
    public override void Delete()
    {
        // deleting directories is not meaningful empty directories are
        // not persistred
    }

    /// <inheritdoc />
    public override FileAttributes Attributes => FileAttributes.Directory;
}