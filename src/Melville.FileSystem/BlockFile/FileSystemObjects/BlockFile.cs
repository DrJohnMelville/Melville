    using System.IO;
using System.Threading.Tasks;
using Melville.FileSystem.BlockFile.BlockMultiStreams;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public class BlockFile(
    BlockDirectory parent, string name, 
    uint startBlock = BlockMultiStream.InvalidBlock, 
    uint endBlock = BlockMultiStream.InvalidBlock,
    uint size = 0) : 
    BlockFileSystemObject(parent, name), IFile, IEndBlockWriteDataTarget
{
    private uint startBlock = startBlock;
    private uint endBlock = endBlock;
    /// <inheritdoc />
    public override bool Exists() => 
        startBlock != BlockMultiStream.InvalidBlock;

    /// <inheritdoc />
    public override void Delete()
    {
        parent.Store.DeleteStream(startBlock, endBlock);
        startBlock = endBlock = BlockMultiStream.InvalidBlock;
    }

    /// <inheritdoc />
    public Task<Stream> OpenRead()
    {
        if (!Exists())
            throw new FileNotFoundException($"File {Path} does not exist.");
        return Task.FromResult(Parent!.Store.GetReader(startBlock, Size));
    }

    /// <inheritdoc />
    public async Task<Stream> CreateWrite(FileAttributes attributes = FileAttributes.Normal)
    {
        return await Parent!.Store.GetWriterAsync(this);
    }

    /// <inheritdoc />
    public void EndStreamWrite(uint startBlock, uint endBlock, long length)
    {
        Delete(); // delete any prior stream data
        this.startBlock = startBlock;
        this.endBlock = endBlock;
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
        target.SendFileData(Name, startBlock,endBlock, Size);
    }
}