    using System.IO;
using System.Threading.Tasks;
using Melville.FileSystem.BlockFile.BlockMultiStreams;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public class BlockFile(
    BlockDirectory parent, string name, 
    StreamEnds streamEnds,
    uint size = 0) : 
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
        parent.Store.DeleteStream(streamEnds);
        streamEnds = StreamEnds.Invalid;
    }

    /// <inheritdoc />
    public Task<Stream> OpenRead()
    {
        if (!Exists())
            throw new FileNotFoundException($"File {Path} does not exist.");
        return Task.FromResult(Parent!.Store.GetReader(streamEnds.Start, Size));
    }

    /// <inheritdoc />
    public async Task<Stream> CreateWrite(FileAttributes attributes = FileAttributes.Normal)
    {
        return await Parent!.Store.GetWriterAsync(this);
    }

    /// <inheritdoc />
    public void EndStreamWrite(in StreamEnds ends, long length)
    {
        Delete(); // delete any prior stream data
        streamEnds = ends;
        Size = length;
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
}