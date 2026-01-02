namespace PddJpegExtractor;

public class DivertingStream(Stream inner, Stream divertTo) : Stream
{
    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => inner.Length;

    public override long Position { 
        get => inner.Position; 
        set => inner.Position = value; }

    public override void Flush() => divertTo.Flush();

    public override int Read(byte[] buffer, int offset, int count) =>
        Read(buffer.AsSpan(offset, count));
    public override int Read(Span<byte> buffer)
    {
        var bytesRead = inner.Read(buffer);
        if (bytesRead > 0) 
            divertTo.Write(buffer.Slice(0, bytesRead));
        return bytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
}
