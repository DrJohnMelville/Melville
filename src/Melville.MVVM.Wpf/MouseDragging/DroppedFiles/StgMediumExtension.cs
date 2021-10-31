using  System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

public static class StgMediumExtension
{
  public static Stream ExtractFileStream(this STGMEDIUM stgmedium)
  {
    if (stgmedium.tymed != TYMED.TYMED_ISTREAM)
    {
      throw new NotSupportedException("Only the stream types are supported for drag and drop");
    }

    var stream = (IStream)Marshal.GetObjectForIUnknown(stgmedium.unionmember);
    Marshal.Release(stgmedium.unionmember);
    return new OleIStreamWrapper(stream);
  }

  private class OleIStreamWrapper : Stream
  {
    private IStream innerStream;
    public OleIStreamWrapper(IStream innerStream)
    {
      this.innerStream = innerStream;
    }

    public override void Flush()
    {
      throw new NotSupportedException();
    }
    public override long Seek(long offset, SeekOrigin origin)
    {
      throw new NotSupportedException();
    }
    public override void SetLength(long value)
    {
      throw new NotSupportedException();
    }
    public override unsafe int Read(byte[] buffer, int offset, int count)
    {
      int value = 0;
      if (offset != 0)
      {
        return AdaptOleHasToReadToBeginning(buffer, offset, count);
      }
      innerStream.Read(buffer, count, (IntPtr)(&value));
      bytesRead += value;
      return value;
    }

    // OLE can only read to the beginning of a buffer, nut .net can read to an offset.  
    // if we need to read to a offset, we recursively read intoa temp buffer and copy it 
    // to the proper place; 
    private int AdaptOleHasToReadToBeginning(byte[] buffer, int offset, int count)
    {
      var tempbuf = new byte[count];
      var len = Read(tempbuf, 0, count);
      Array.Copy(tempbuf, 0, buffer, offset, len);
      return len;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      throw new NotSupportedException();
    }
    public override bool CanRead
    {
      get { return true; }
    }
    public override bool CanSeek
    {
      get { return false; }
    }
    public override bool CanWrite
    {
      get { return false; }
    }
    public override long Length
    {
      get
      {
        innerStream.Stat(out var streamStat, 0);
        return streamStat.cbSize;
      }
    }
    private long bytesRead = 0;
    public override long Position
    {
      get => bytesRead;
      set { throw new NotSupportedException(); }
    }
  }
}