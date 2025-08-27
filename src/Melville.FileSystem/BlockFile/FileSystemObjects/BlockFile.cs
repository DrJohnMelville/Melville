    using System.Diagnostics;
    using System.IO;
    using System.IO.Pipelines;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
using Melville.FileSystem.BlockFile.BlockMultiStreams;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public class BlockFile(BlockDirectory parent, string name, long size) : 
    BlockFileSystemObject(parent, name), IFile, IEndBlockDataTarget
{
    public long Size { get; set; } = size;
    
    public new BlockDirectory Parent => base.Parent!;

    /// <inheritdoc />
    public override bool Exists() => Parent!.TryGetFile(Name, out var value) && value.Exists();

    /// <inheritdoc />
    public override void Delete()
    {
        Parent.CreateOrUpdateFile(Name, StreamDescription.Invalid);
    }

    /// <inheritdoc />
    public async Task<Stream> OpenRead()
    {
        if (!Parent.TryGetFile(Name, out var description))
            throw new FileNotFoundException($"File {Path} does not exist.");
        return await Task.FromResult(Parent.Store.GetReader(description.Start, description.Length, this));
    }

    /// <inheritdoc />
    public async Task<Stream> CreateWrite(FileAttributes attributes = FileAttributes.Normal)
    {
        await (Parent.Root?.BeginWriteStream() ?? Task.CompletedTask);
        return await Parent.Store.GetWriterAsync(this);
    }

    /// <inheritdoc />
    public void EndStreamWrite(in StreamEnds ends, long length)
    {
        Parent.CreateOrUpdateFile(Name, new StreamDescription(ends.Start, ends.End, length));
        Parent.Root?.EndWriteStream();
    }

    /// <inheritdoc />
    public void EndStreamRead() => Parent.Root?.EndReadStream();

    /// <inheritdoc />
    public byte FinalProgress => 255;

    /// <inheritdoc />
    public Task WaitForFinal => Task.CompletedTask;

    public override FileAttributes Attributes => FileAttributes.Normal;

}