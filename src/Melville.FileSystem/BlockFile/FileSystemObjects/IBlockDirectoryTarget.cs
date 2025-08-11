using System;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Melville.FileSystem.BlockFile.BlockMultiStreams;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public interface IBlockDirectoryTarget: IAsyncDisposable
{
    void SendFolderName(string name);
    void SendFolderCount(uint count);
    void SendFileCount(uint count);
    void SendFileData(string name, in StreamEnds ends, long size);
    ValueTask FlushAsync();
}

public class HalfBlockDirectoryTarget(Stream offsets, uint namesBlock) : IBlockDirectoryTarget
{
    private readonly PipeWriter offsetsPipe =
        CreateOffsetsWriter(offsets, namesBlock);

    private static PipeWriter CreateOffsetsWriter(Stream offsets, uint namesBlock)
    {
        var ret = PipeWriter.Create(offsets);
        var buffer = ret.GetSpan(4);
        MemoryMarshal.Cast<byte, uint>(buffer)[0] = namesBlock;
        ret.Advance(4);
        return ret;
    }

    public virtual void SendFolderName(string name)
    {
    }
    /// <inheritdoc />
    public virtual void SendFolderCount(uint count)
    {
    }
    /// <inheritdoc />
    public virtual void SendFileCount(uint count)
    {
    }
    /// <inheritdoc />
    public virtual void SendFileData(string name, in StreamEnds ends, long size)
    {
        var offsets = offsetsPipe.GetSpan(16);
        var nums = MemoryMarshal.Cast<byte, uint>(offsets);
        nums[0] = ends.Start;
        nums[1] = ends.End;
        MemoryMarshal.Cast<byte, long>(offsets)[1] = size;
        offsetsPipe.Advance(16);
    }
    public virtual async ValueTask FlushAsync()
    {
        await offsetsPipe.FlushAsync();
    }
    /// <inheritdoc />
    public virtual async  ValueTask DisposeAsync()
    {
        await FlushAsync();
        await offsets.DisposeAsync();
    }
}

public class FullBlockDirectoryTarget(
    Stream names, Stream offsets, uint namesBlock) : HalfBlockDirectoryTarget(offsets, namesBlock)
{
    private readonly PipeWriter namePipe = PipeWriter.Create(names);

    public override void SendFolderName(string name) => namePipe.WriteEncodedString(name);

    /// <inheritdoc />
    public override void SendFolderCount(uint count) => namePipe.WriteCompactUint(count);

    /// <inheritdoc />
    public override void SendFileCount(uint count) => namePipe.WriteCompactUint(count);

    /// <inheritdoc />
    public override void SendFileData(string name, in StreamEnds ends, long size)
    {
        namePipe.WriteEncodedString(name);
        base.SendFileData(name, ends, size);
    }

    public override async ValueTask FlushAsync()
    {
        await namePipe.FlushAsync();
        await base.FlushAsync();
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        await FlushAsync();
        await names.DisposeAsync();
        await base.DisposeAsync();
    }
}