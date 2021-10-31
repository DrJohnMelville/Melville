using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.Log.Viewer.LogViews;

public class AsyncToSyncProgressStream : Stream
{
    private Stream inner;
    private readonly CancellationToken cnacellationToken;
    private Queue<(int Lenght, byte[] Data)> buffer = new Queue<(int, byte[])>();
    private (int Length, byte[] Data) currentBuffer = (0,Array.Empty<byte>());
    private int currentBufferPosition = 0;
    private volatile Task<bool> nextRead = Task.FromResult(true);

    public AsyncToSyncProgressStream(Stream inner, CancellationToken cnacellationToken)
    {
        this.inner = inner;
        this.cnacellationToken = cnacellationToken;
        ReadSource();
    }

    private async void ReadSource()
    {
        do
        {
            nextRead = InnerRead();
        } while (await nextRead);
    }

    private async Task<bool> InnerRead()
    {
        try
        {
            var innerBuffer = new byte[512];
            var bytesRead = await inner.ReadAsync(innerBuffer, 0, innerBuffer.Length, cnacellationToken);
            if (bytesRead == 0) return false;
            buffer.Enqueue((bytesRead, innerBuffer));
            return true;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    public bool HasDataToRead => buffer.Count > 0 || HasDataInCurrentBuffer();

    private bool HasDataInCurrentBuffer() => currentBuffer.Length > currentBufferPosition;

    public Task<bool> WaitForData() => HasDataToRead ? Task.FromResult(true) : nextRead;
        

    public override void Flush() => throw new NotSupportedException();

    public override int Read(byte[] buffer, int offset, int count)
    {
        var localBytesRead = 0;
        while (count > 0 && HasDataToRead)
        {
            if (!HasDataInCurrentBuffer()) RefreshCurrentBuffer();
            var bytesToCopy = Math.Min(count, currentBuffer.Length - currentBufferPosition);
            Array.Copy(currentBuffer.Data, currentBufferPosition, buffer, offset, bytesToCopy);
            offset += bytesToCopy;
            localBytesRead += bytesToCopy;
            currentBufferPosition += localBytesRead;
            count -= bytesToCopy;
        }

        return localBytesRead;
    }

    private void RefreshCurrentBuffer()
    {
        currentBuffer = buffer.Dequeue();
        currentBufferPosition = 0;
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => inner.Length;
    public override long Position { get; set; }
}