using Melville.FileSystem.BlockFile.BlockMultiStreams;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public class BlockDirectory(BlockDirectory? parent, string name): 
    BlockFileSystemObject(parent, name), IDirectory
{
    public virtual BlockMultiStream Store => 
        Parent?.Store ?? throw new ArgumentNullException(nameof(Parent));

    private ConcurrentDictionary<string, BlockDirectory> directories = new();
    private ConcurrentDictionary<string, StreamDescription> files = new();

    internal bool TryGetFile(string key, out StreamDescription value) => 
        files.TryGetValue(key, out value);

    internal void CreateOrUpdateFile(string key, StreamDescription file)
    {
        var full = false;
        files.AddOrUpdate(key, _ =>
        {
            full = true;
            return file;
        }, (_, old) =>
        {
            Debug.Assert(file.StreamEnds != old.StreamEnds);
            Store.DeleteStream(old.StreamEnds); // deletestream will only delete valid streams
            return file;
        });
        // we do this after the AddOrUpdate to make sure the file exists that needs to be rewritten
        if (full)
            Root?.TriggerFullRewrite();
        else
            Root?.TriggerRewrite();
    }

    public virtual BlockRootDirectory? Root => Parent?.Root;

    IDirectory IDirectory.SubDirectory(string name) => 
        SubDirectory(name);

    public BlockDirectory SubDirectory(string name) => 
        directories.GetOrAdd(name, i => new BlockDirectory(this, i));

    /// <inheritdoc />
    IFile IDirectory.File(string name) => File(name);
    public BlockFile File(string name)
    {
        var size = TryGetFile(name, out  var desc) ? desc.Length : 0;
        return new BlockFile(this, name, size);
    }

    /// <inheritdoc />
    public IEnumerable<IFile> AllFiles() => 
        files.Select(i => new BlockFile(this, i.Key, i.Value.Length));

    /// <inheritdoc />
    public IEnumerable<IFile> AllFiles(string glob)
    {
        var regex = new Regex($"^{RegexExtensions.GlobToRegex(glob)}$");
        return files
            .Where(i=>regex.IsMatch(i.Key))
            .Select(i => new BlockFile(this, i.Key, i.Value.Length));
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
        var dirToWrite = directories
            .Where(i=>i.Value.HasContent())
            .OrderBy(i=>i.Key).ToArray();
        target.SendFolderCount((uint)dirToWrite.Length);
        foreach (var dir in dirToWrite)
        {
            target.SendFolderName(dir.Key);
            await dir.Value.WriteToAsync(target);
        }
        var filesToWrite = files.OrderBy(i=>i.Key).ToArray();
        target.SendFileCount((uint)files.Count);
        foreach (var file in filesToWrite)
        {
            target.SendFileData(file.Key, file.Value.StreamEnds, file.Value.Length);
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
                var description = await ReadOffsets(offsetReader);
                CreateOrUpdateFile(name, description);
            }
    }

    public async ValueTask<StreamDescription> ReadOffsets(PipeReader offsetReader)
    {
        var result = await offsetReader.ReadAtLeastAsync(16);
        var span = result.Buffer.MinSpan(stackalloc byte[16]);

        var uints = MemoryMarshal.Cast<byte, uint>(span);
        var size = MemoryMarshal.Cast<byte, long>(span)[1];
        
        var ret = new StreamDescription(uints[0], uints[1], size);
        offsetReader.AdvanceTo(result.Buffer.GetPosition(16));
        return ret;
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
        
        new DictionaryCleaner<string, StreamDescription>(files, f=>!f.Value.Exists()).DoCull();
    }

    private bool HasContent() => !files.IsEmpty || AllSubDirectories().Any(i=>HasContent());
}