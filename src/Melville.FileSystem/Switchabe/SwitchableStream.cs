using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.Switchabe;

public sealed class SwitchableStream : DelegatingStream
{
  private readonly SemaphoreSlim readerLock = new SemaphoreSlim(1,1);

  public SwitchableStream(Stream inner) : base(inner)
  {
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    readerLock.Wait();
    try
    {
      return base.Seek(offset, origin);
    }
    finally
    {
      readerLock.Release();
    }
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    readerLock.Wait();
    try
    {
      return base.Read(buffer, offset, count);
    }
    finally
    {
      readerLock.Release();
    }
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    readerLock.Wait();
    try
    {
      base.Write(buffer, offset, count);
    }
    finally
    {
      readerLock.Release();
    }
  }

  public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
  {
    await readerLock.WaitAsync();
    try
    {
      return await base.ReadAsync(buffer, offset, count, cancellationToken);
    }
    finally
    {
      readerLock.Release();
    }
  }

  public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
  {
    await readerLock.WaitAsync();
    try
    {
      await base.WriteAsync(buffer, offset, count, cancellationToken);
    }
    finally
    {
      readerLock.Release();
    }
  }

  public override async Task FlushAsync(CancellationToken cancellationToken)
  {
    await readerLock.WaitAsync();
    try
    {
      await base.FlushAsync(cancellationToken);
    }
    finally
    {
      readerLock.Release();
    }
  }

  private bool isClosed = false;
  public override void Close()
  {
    isClosed = true;
    base.Close();
  }

  protected override void Dispose(bool disposing)
  {
    if (isClosed) return;
    isClosed = true;
    readerLock.Release(); // we try to own the lock before we dispose it, ensureing we do not dispose before switchSource returns;
    readerLock.Dispose();
    base.Dispose(disposing);
  }

  public async Task SwitchSource(Func<Task<Stream>> streamFactory)
  {
    if (isClosed) return;
    try
    {
      var newInner = await streamFactory();
      if (Inner == newInner) throw new InvalidOperationException("fdgq");
      await readerLock.WaitAsync();
      var pos = Inner.Position;
      newInner.Seek(pos, SeekOrigin.Begin);
      var oldInner = Inner;
      Inner = newInner;
      //oldInner?.Dispose();
    }
    catch (ObjectDisposedException)
    {
      // can happen when the new stream is already closed
    }
    finally
    {
      readerLock.Release();
    }
  }
}