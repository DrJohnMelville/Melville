using Melville.FileSystem.BlockFile.BlockMultiStreams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Parent?.Store ?? throw new ArgumentNullException(nameof(Parent));

    private SortedDictionary<string, BlockDirectory> directories = new();
    private SortedDictionary<string, BlockFile> files = new();

    public virtual BlockRootDirectory? Root => Parent?.Root;

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
        Root?.TriggerFullRewrite();
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
    public IFile FileFromRawPath(string path) => throw new NotSupportedException();

    /// <inheritdoc />
    public bool IsVolitleDirectory() => false;

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
        var dirToWrite = directories.ToArray();
        target.SendFolderCount((uint)dirToWrite.Length);
        foreach (var dir in dirToWrite)
        {
            target.SendFolderName(dir.Key);
            await dir.Value.WriteToAsync(target);
        }
        var filesToWrite = files.ToArray();
        target.SendFileCount((uint)filesToWrite.Length);
        foreach (var file in filesToWrite)
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

    protected void CullDeletedFiles()
    {
        foreach (var dir in directories.Values)
        {
            dir.CullDeletedFiles();
        }

        new DictionaryCleaner<string, BlockFile>(files, f=>!f.Value.Exists()).DoCull();
    }
}