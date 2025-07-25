using System;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public class BlockStreamReader(BlockMultiStream data, uint firstBlock, long length)
    : BlockStream(data, firstBlock, length)
{
    public override bool CanRead => true;
    public override bool CanWrite => false;

    public override async ValueTask<int>
        ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = new CancellationToken())
    {
        var partialBuffer = buffer;
        int localRead;
        do
        {
            await TryAdvanceBlockAsync();
            localRead = await Data.ReadFromBlockDataAsync(
                TrimBufferToStreamLength(partialBuffer), CurrentBlock, CurrentBlockOffset);
            Position += localRead;
            partialBuffer = partialBuffer[localRead..];
        } while (localRead > 0 && partialBuffer.Length > 0);

        return buffer.Length - partialBuffer.Length;
    }

    private Memory<byte> TrimBufferToStreamLength(Memory<byte> partialBuffer)
    {
        var remainingLength = Length - Position;
        return remainingLength > partialBuffer.Length
            ? partialBuffer
            : partialBuffer[..(int)remainingLength];
    }

    private ValueTask TryAdvanceBlockAsync() =>
        AnotherBlockNeeded() ? AdvanceBlockAsync() : ValueTask.CompletedTask;

    private bool AnotherBlockNeeded() => DataRemainingInBlock <= 0 && Position < Length;

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Flush()
    {
    }

    public override Task FlushAsync(System.Threading.CancellationToken cancellationToken)
    {
        Flush();
        return Task.CompletedTask;
    }
}