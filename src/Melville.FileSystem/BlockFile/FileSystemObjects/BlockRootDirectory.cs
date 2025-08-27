using System;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Melville.FileSystem.BlockFile.BlockMultiStreams;
using Melville.FileSystem.PseudoTransactedFS;
using Melville.Hacks;
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
    public override BlockRootDirectory? Root => this;

    private bool RewriteNeeded { get; set; } = true;
    private bool FullRewriteNeeded { get; set; } = true;

    #region  synchronization

    private readonly SemaphoreSlim writeLock = new(1);

    public Task BeginReadStream()
    {
        return Task.CompletedTask;
    }
    public Task BeginWriteStream()
    {
        return writeLock.WaitAsync();
    }

    public void EndReadStream()
    {

    }
    public void EndWriteStream()
    {
        writeLock.Release();
    }

    public void TriggerRewrite() => RewriteNeeded = true;

    public void TriggerFullRewrite() => FullRewriteNeeded = RewriteNeeded = true;
    #endregion

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
        using var _ = await writeLock.WaitForHandleAsync();
        CullDeletedFiles();
        await using var nameStream = await store.GetWriterAsync(
            NullEndBlockDataTarget.Instance);
        await using var offsetStream = await store.GetWriterAsync(
            NullEndBlockDataTarget.Instance);
        CheckForDeletedBlocks(nameStream, offsetStream);
        var target = new NameAndOffsetWritingDirectoryTarget(
            nameStream, offsetStream, nameStream.FirstBlock);
        CheckForDeletedBlocks(nameStream, offsetStream);
        await WriteToAsync(target);
        CheckForDeletedBlocks(nameStream, offsetStream);
        await target.FlushAsync();
        CheckForDeletedBlocks(nameStream, offsetStream);
        store.DeleteStream(nameLocation);
        store.DeleteStream(offsetsLocation);
        await store.WriteHeaderBlockAsync(offsetStream.FirstBlock);
        nameLocation = nameStream.StreamEnds();
        offsetsLocation = offsetStream.StreamEnds();
    }

    [Conditional("DEBUG")]
    private void CheckForDeletedBlocks(BlockWritingStream nameStream, BlockWritingStream offsetStream)
    {
        store.AssertNotDeleted(offsetsLocation);
        store.AssertNotDeleted(nameLocation);
        store.AssertNotDeleted(nameStream.CurrentExtent());
        store.AssertNotDeleted(offsetStream.CurrentExtent());
    }

    private async ValueTask WriteOffsetsOnly()
    {
        using var _ = await writeLock.WaitForHandleAsync();
        await using var offsetStream = await store.GetWriterAsync(
            NullEndBlockDataTarget.Instance);
        Debug.Assert(nameLocation.Start != offsetStream.StreamEnds().Start);
        var target = new OffsetWritingDirectoryTarget(offsetStream, nameLocation.Start);
        await WriteToAsync(target);
        await target.FlushAsync();
        store.DeleteStream(offsetsLocation);
        await store.WriteHeaderBlockAsync(offsetStream.FirstBlock);
        offsetsLocation = offsetStream.StreamEnds();
    }

    public async ValueTask ReadFromStore()
    {
        using var _ = await writeLock.WaitForHandleAsync();
        if (store.RootBlock == BlockMultiStream.InvalidBlock)
            return; // Nothing to read
        await using var offsetReader = store.GetReader(store.RootBlock, long.MaxValue, NullEndBlockDataTarget.Instance);
        var offsetPipe = PipeReader.Create(offsetReader);
        var nameOffset = await offsetPipe.ReadUintBySize<uint>(4);
        Debug.Assert(store.RootBlock != nameOffset);
        await using var nameReader = store.GetReader(nameOffset, long.MaxValue, NullEndBlockDataTarget.Instance);
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

