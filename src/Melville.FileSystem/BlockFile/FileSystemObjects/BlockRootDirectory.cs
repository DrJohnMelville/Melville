using System;
using System.Buffers;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Melville.FileSystem.BlockFile.BlockMultiStreams;
using Melville.FileSystem.PseudoTransactedFS;
using Melville.INPC;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public partial class BlockTransaction(BlockRootDirectory dir) : ITransactedDirectory
{
    [DelegateTo] private IDirectory Inner => dir;

    /// <inheritdoc />
    public ValueTask Commit() => dir.Commit();

    /// <inheritdoc />
    public void Rollback()
    {
        throw new NotSupportedException(
            "Should not be explicitly rolling back this directory.");
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
public class BlockRootDirectory(BlockMultiStream store) : 
    BlockDirectory(null, ""), IDisposable
{
    public override string Path => "";
    public override BlockMultiStream Store => store;

    private StreamEnds nameLocation = StreamEnds.Invalid;
    private StreamEnds offsetsLocation = StreamEnds.Invalid;

    public override bool RewriteNeeded { get; set; } = true;
    public override bool FullRewriteNeeded { get; set; } = true;
    
    public ValueTask Commit()
    {
        var ret = (RewriteNeeded, FullRewriteNeeded) switch
        {
            (_, true) => WriteNamesAndOffsets(),
            (true, false) => WriteOffsetsOnly(),
            _ => ValueTask.CompletedTask
        };
        // I do not care about the order of the rewrite and resetting the flags so save myself a state machine
        ClearRewriteTags();
        return ret;
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
        if (store.RootBlock == BlockMultiStream.InvalidBlock)
            return; // Nothing to read
        await using var offsetReader = store.GetReader(store.RootBlock, long.MaxValue);
        var offsetPipe = PipeReader.Create(offsetReader);
        var nameOffset = await offsetPipe.ReadUintBySize<uint>(4);
        await using var nameReader = store.GetReader(nameOffset, long.MaxValue);
        var namePipe = PipeReader.Create(nameReader);
        await ReadFromStreams(namePipe, offsetPipe);

        nameLocation = nameReader.CurrentExtent();
        offsetsLocation = offsetReader.CurrentExtent();

        await namePipe.CompleteAsync();
        await offsetPipe.CompleteAsync();

        ClearRewriteTags();
    }

    /// <inheritdoc />
    public void Rollback()
    {
        Debug.Fail("Should not be explicitly rolling back this directory");
    }

    /// <inheritdoc />
    public void Dispose() => store.Dispose();
}