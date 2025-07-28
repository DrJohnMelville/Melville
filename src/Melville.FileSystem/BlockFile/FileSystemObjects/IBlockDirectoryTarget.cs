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

public class FullBlockDirectoryTarget(
    Stream names, Stream offsets, uint namesBlock) : IBlockDirectoryTarget
{
    private readonly PipeWriter namePipe = PipeWriter.Create(names);
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

    public void SendFolderName(string name)
    {
        namePipe.WriteEncodedString(name);
    }

    /// <inheritdoc />
    public void SendFolderCount(uint count)
    {
        namePipe.WriteCompactUint(count);

    }

    /// <inheritdoc />
    public void SendFileCount(uint count)
    {
        namePipe.WriteCompactUint(count);
    }

    /// <inheritdoc />
    public void SendFileData(string name, in StreamEnds ends, long size)
    {
        namePipe.WriteEncodedString(name);
        var offsets = offsetsPipe.GetSpan(16);
        var nums = MemoryMarshal.Cast<byte, uint>(offsets);
        nums[0] = ends.Start;
        nums[1] = ends.End;
        MemoryMarshal.Cast<byte, long>(offsets)[1] = size;
        offsetsPipe.Advance(16);

    }

    public async ValueTask FlushAsync()
    {
        await namePipe.FlushAsync();
        await offsetsPipe.FlushAsync();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await FlushAsync();
        await names.DisposeAsync();
        await offsets.DisposeAsync();
    }
}