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

    public override bool RewriteNeeded { get; set; } = true;
    public override bool FullRewriteNeeded { get; set; } = true;
    
    public async ValueTask WriteToStore()
    {
        if (!(RewriteNeeded || FullRewriteNeeded)) return;
        await (FullRewriteNeeded ?WriteNamesAndOffsets(): WriteOffsetsOnly());
        ClearRewriteTags();
    }

    private void ClearRewriteTags()
    {
        RewriteNeeded = false;
        FullRewriteNeeded = false;
    }

    private async ValueTask WriteNamesAndOffsets()
    {
        await using var nameStream = await store.GetWriterAsync(
            NullEndBlockWriteDataTarget.Instance);
        await using var offsetStream = await store.GetWriterAsync(
            NullEndBlockWriteDataTarget.Instance);
        var target = new NameAndOffsetWritingDirectoryTarget(
            nameStream, offsetStream, nameStream.FirstBlock);
        await WriteToAsync(target);
        await target.FlushAsync();
        store.DeleteStream(nameLocation);
        store.DeleteStream(offsetsLocation);
        await store.WriteHeaderBlockAsync(offsetStream.FirstBlock);
        nameLocation = nameStream.StreamEnds();
        offsetsLocation = offsetStream.StreamEnds();
    }

    private async ValueTask WriteOffsetsOnly()
    {
        await using var offsetStream = await store.GetWriterAsync(
            NullEndBlockWriteDataTarget.Instance);
        var target = new OffsetWritingDirectoryTarget(offsetStream, nameLocation.Start);
        await WriteToAsync(target);
        await target.FlushAsync();
        store.DeleteStream(offsetsLocation);
        await store.WriteHeaderBlockAsync(offsetStream.FirstBlock);
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
        ClearRewriteTags();
    }
}