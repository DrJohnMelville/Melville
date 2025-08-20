    using System.Diagnostics;
    using System.IO;
    using System.IO.Pipelines;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
using Melville.FileSystem.BlockFile.BlockMultiStreams;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public class BlockFile(
    BlockDirectory parent, string name, 
    StreamEnds streamEnds,
    long size = 0) : 
    BlockFileSystemObject(parent, name), IFile, IEndBlockWriteDataTarget
{
    public BlockFile(BlockDirectory parent, string name) :
        this(parent, name, StreamEnds.Invalid, 0)
    {
    }

    private StreamEnds streamEnds = streamEnds;
    /// <inheritdoc />
    public override bool Exists() => 
        streamEnds.IsValid();

    /// <inheritdoc />
    public override void Delete()
    {
        Parent?.Store.DeleteStream(streamEnds);
        ForceDirectoryRewrite();
        streamEnds = StreamEnds.Invalid;
    }

    /// <inheritdoc />
    public async Task<Stream> OpenRead()
    {
        if (!Exists())
            throw new FileNotFoundException($"File {Path} does not exist.");
        return await Task.FromResult(Parent!.Store.GetReader(streamEnds.Start, Size));
    }

    /// <inheritdoc />
    public async Task<Stream> CreateWrite(FileAttributes attributes = FileAttributes.Normal)
    {
        return await Parent!.Store.GetWriterAsync(this);
    }

    /// <inheritdoc />
    public void EndStreamWrite(in StreamEnds ends, long length)
    {
        Debug.Assert(streamEnds != ends, "Invalid delete -- perhaps from double closing the writing stream.");
        Delete(); // delete any prior stream data
        ForceDirectoryRewrite();
        streamEnds = ends;
        Size = length;
    }

    private void ForceDirectoryRewrite()
    {
        if (Parent is not null)
            Parent.RewriteNeeded = true;
    }

    /// <inheritdoc />
    public byte FinalProgress => 255;

    /// <inheritdoc />
    public Task WaitForFinal => Task.CompletedTask;

    /// <inheritdoc />
    public long Size { get; set; } = size;

    public override FileAttributes Attributes => FileAttributes.Normal;

    public void WriteFileTo(IBlockDirectoryTarget target)
    {
        target.SendFileData(Name, streamEnds, Size);
    }

    public async ValueTask ReadOffsets(PipeReader offsetReader)
    {
        var result = await offsetReader.ReadAtLeastAsync(16);
        var span = result.Buffer.MinSpan(stackalloc byte[16]);
        var uints = MemoryMarshal.Cast<byte, uint>(span);
        streamEnds = new StreamEnds(uints[0], uints[1]);
        Size = MemoryMarshal.Cast<byte, long>(span)[1];
        offsetReader.AdvanceTo(result.Buffer.GetPosition(16));
    }
}