using  System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Melville.MVVM.Invariants;

namespace Melville.MVVM.FileSystem
{
  public sealed class ExtendedCloseStream : DelegatingStream
  {
    private Action? closeAction;
    public ExtendedCloseStream(Stream inner, Action closeAction) : base(inner)
    {
      this.closeAction = closeAction;
    }

    public override void Close()
    {
      base.Close();
      GC.SuppressFinalize(this);
      // only call the extended close action once
      var localAction = closeAction;
      closeAction = null;
      if (localAction == null) return;
      localAction();
    }

    ~ExtendedCloseStream()
    {
      Close();
    }
  }

  public abstract class DelegatingStream : Stream
  {
    protected Stream Inner;
    public DelegatingStream(Stream inner)
    {
      Inner = inner;
    }

    #region delegating functions

    protected override void Dispose(bool disposing)
    {
      Inner.Dispose();
      base.Dispose(disposing);
    }
    public override void Close()
    {
      Inner.Close();
      base.Close();
    }

    public override void Flush()
    {
      Inner.Flush();
    }
    public override long Seek(long offset, SeekOrigin origin)
    {
      return Inner.Seek(offset, origin);
    }
    public override void SetLength(long value)
    {
      Inner.SetLength(value);
    }
    public override int Read(byte[] buffer, int offset, int count)
    {
      return Inner.Read(buffer, offset, count);
    }
    public override void Write(byte[] buffer, int offset, int count)
    {
      Inner.Write(buffer, offset, count);
    }
    public override bool CanRead
    {
      get { return Inner.CanRead; }
    }
    public override bool CanSeek
    {
      get { return Inner.CanSeek; }
    }
    public override bool CanWrite
    {
      get { return Inner.CanWrite; }
    }
    public override long Length
    {
      get { return Inner.Length; }
    }
    public override long Position
    {
      get { return Inner.Position; }
      set { Inner.Position = value; }
    }
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
      return Inner.ReadAsync(buffer, offset, count, cancellationToken);
    }
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
      return Inner.WriteAsync(buffer, offset, count, cancellationToken);
    }
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      return Inner.FlushAsync(cancellationToken);
    }
    public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
    {
      var readBufer = new byte[bufferSize];
      var writeBuffer = new byte[bufferSize];
      int bytesToWrite = 0;

      do
      {
        var readHandle = ReadAsync(readBufer, 0, bufferSize);
        if (bytesToWrite > 0)
        {
          await destination.WriteAsync(writeBuffer, 0, bytesToWrite);
        }
        bytesToWrite = await readHandle;
        var temp = writeBuffer;
        writeBuffer = readBufer;
        readBufer = temp;
      } while (bytesToWrite > 0);
    }
    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object? state)
    {
      return Inner.BeginRead(buffer, offset, count, callback, state);
    }
    public override int EndRead(IAsyncResult asyncResult)
    {
      return Inner.EndRead(asyncResult);
    }
    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object? state)
    {
      return Inner.BeginWrite(buffer, offset, count, callback, state);
    }
    public override void EndWrite(IAsyncResult asyncResult)
    {
      Inner.EndWrite(asyncResult);
    }
    public override bool CanTimeout
    {
      get { return Inner.CanTimeout; }
    }
    public override int ReadTimeout
    {
      get { return Inner.ReadTimeout; }
      set { Inner.ReadTimeout = value; }
    }
    public override int WriteTimeout
    {
      get { return Inner.WriteTimeout; }
      set { Inner.WriteTimeout = value; }
    }
    #endregion
  }



}