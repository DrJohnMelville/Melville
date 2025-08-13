using Melville.FileSystem.BlockFile.BlockMultiStreams;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public class BlockDirectory(BlockDirectory? parent, string name): 
    BlockFileSystemObject(parent, name), IDirectory
{
    public virtual BlockMultiStream Store => 
        parent?.Store ?? throw new ArgumentNullException(nameof(parent));

    private SortedDictionary<string, BlockDirectory> directories = new();
    private SortedDictionary<string, BlockFile> files = new();

    public virtual bool RewriteNeeded
    {
        get => Parent?.RewriteNeeded ?? true;
        set
        {
            if (Parent is not null)
              Parent.RewriteNeeded = value;
        }
    }
    public virtual bool FullRewriteNeeded
    {
        get => Parent?.FullRewriteNeeded ?? true;
        set
        {
            if (Parent is not null)
              Parent.FullRewriteNeeded = value;
        }
    }


    IDirectory IDirectory.SubDirectory(string name) => 
        SubDirectory(name);

    public BlockDirectory SubDirectory(string name)
    {  
        if (directories.TryGetValue(name, out var result))
            return result;
        var newDir = new BlockDirectory(this, name);
        directories.Add(name, newDir);
        return newDir;
    }

    /// <inheritdoc />
    IFile IDirectory.File(string name) => File(name);
    public BlockFile File(string name)
    {
        if (files.TryGetValue(name, out var result))
            return result;
        var newFile = new BlockFile(this, name);
        files.Add(name, newFile);
        RewriteNeeded = true;
        FullRewriteNeeded = true;
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

    public async ValueTask WriteToAsync(IBlockDirectoryTarget target)
    {
        target.SendFolderCount((uint)directories.Count);
        foreach (var dir in directories)
        {
            target.SendFolderName(dir.Key);
            await dir.Value.WriteToAsync(target);
        }
        target.SendFileCount((uint)files.Count);
        foreach (var file in files)
        {
            file.Value.WriteFileTo(target);
        }
    }

    protected async ValueTask ReadFromStreams(PipeReader nameReader, PipeReader offsetReader)
    {
            var folderCount = await nameReader.ReadCompactUint();
            await ReadDirectories(nameReader, offsetReader, folderCount);

            var fileCount = (int)await nameReader.ReadCompactUint();
            for (int i = 0; i < fileCount; i++)
            {
                var name = await nameReader.ReadEncodedString();
                var file = File(name);
                await file.ReadOffsets(offsetReader);
            }
    }

    private async ValueTask ReadDirectories(PipeReader nameReader, PipeReader offsetReader, uint folderCount)
    {
        for (int i = 0; i < folderCount; i++)
        {
            var name = await nameReader.ReadEncodedString();
            var subdir = SubDirectory(name);
            await subdir.ReadFromStreams(nameReader, offsetReader);
        }
    }
}

public readonly struct DictonaryCleaner<TKey, TItem>(
    IDictionary<TKey, TItem> dicctionary,
    Func<KeyValuePair<TKey, TItem>, bool> shouldCull)
{
    public void DoCull()
    {
        var iter = dicctionary.GetEnumerator();
        while (iter.MoveNext())
        {
            if (!shouldCull(iter.Current)) continue;
            RecursiveCull(iter);
            return;
        }
    }

    private void RecursiveCull(IEnumerator<KeyValuePair<TKey, TItem>> iter)
    {
#warning -- check some really big deletes to make sure they do not blow the stack
        var key = iter.Current.Key;
        while (iter.MoveNext())
        {
            if (!shouldCull(iter.Current)) continue;
            RecursiveCull(iter);
            break;
        }
        dicctionary.Remove(key);
    }
}