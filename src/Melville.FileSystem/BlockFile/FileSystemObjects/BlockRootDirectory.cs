using System.IO.Pipelines;
using System.Threading.Tasks;
using Melville.FileSystem.BlockFile.BlockMultiStreams;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public class BlockRootDirectory(BlockMultiStream store) : 
    BlockDirectory(null, "")
{
    public override string Path => "";
    public override BlockMultiStream Store => store;

    private StreamEnds nameLocation = StreamEnds.Invalid;
    private StreamEnds offsetsLocation = StreamEnds.Invalid;

    public override bool RewriteNeeded { get; set; }
    public override bool FullRewriteNeeded { get; set; }
    
    public async ValueTask WriteToStore()
    {
        // next step is to do half or full block writes as required.
        await using var nameStream = await store.GetWriterAsync(
            NullEndBlockWriteDataTarget.Instance);
        await using var offsetStream = await store.GetWriterAsync(
            NullEndBlockWriteDataTarget.Instance);
        var target = new FullBlockDirectoryTarget(
            nameStream, offsetStream, nameStream.FirstBlock);
        await WriteToAsync(target);
        await target.FlushAsync();
        store.DeleteStream(nameLocation);
        store.DeleteStream(offsetsLocation);
        await store.WriteHeaderBlockAsync(offsetStream.FirstBlock);
        nameLocation = nameStream.StreamEnds();
        offsetsLocation = offsetStream.StreamEnds();
    }

    public async ValueTask ReadFromStore()
    {
        var offsetReader = PipeReader.Create(
            store.GetReader(store.RootBlock, long.MaxValue));
        var nameOffset = await offsetReader.ReadUintBySize<uint>(4);
        var nameReader = PipeReader.Create(
            store.GetReader(nameOffset, long.MaxValue));
        await ReadFromStreams(nameReader, offsetReader);
    }
}