using  System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

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
            // the default span read implementation pulls a bufffer out of the shared pool
            // so we just reuse that.
            return this.Read(buffer.AsSpan(offset, count));
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

    public static IStream WrapWithComIStream(this Stream s) => new ComIStreamWrapper(s);

    // Not this only supports reading and seeking streams
    private class ComIStreamWrapper : IStream
    {
        private Stream? stream;

        public ComIStreamWrapper(Stream stream)
        {
            this.stream = stream;
        }

        /// <inheritdoc />
        public void Clone(out IStream ppstm) => throw new NotSupportedException();

        /// <inheritdoc />
        public void Commit(int grfCommitFlags) => throw new NotSupportedException();

        /// <inheritdoc />
        public unsafe void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            var buffer = ArrayPool<byte>.Shared.Rent((int)cb);
            try
            {
                var bytesRead = stream?.ReadAtLeast(buffer.AsSpan(0, (int)cb), (int)cb, false) ?? 0;
                int bytesWritten = 0;
                pstm.Write(buffer, bytesRead, new IntPtr(&bytesWritten));
                if (pcbRead != IntPtr.Zero)
                {
                    Marshal.WriteInt32(pcbRead, bytesRead);
                }
                if (pcbWritten != IntPtr.Zero)
                {
                    Marshal.WriteInt32(pcbWritten, bytesWritten);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        /// <inheritdoc />
        public void LockRegion(long libOffset, long cb, int dwLockType) => throw new NotSupportedException();

        /// <inheritdoc />
        public void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            var bytesRead = stream?.ReadAtLeast(pv.AsSpan(0, cb),cb, false) ?? 0;
            if (pcbRead != IntPtr.Zero)
            {
                Marshal.WriteInt32(pcbRead, bytesRead);
            }

            if (bytesRead < cb) CloseInnerStream();
        }

        private void CloseInnerStream()
        {
            //the inner stream might be holding on to something expensive, like a database connection.
            // we close it once we try to read past the end of the stream.
            stream?.Dispose();
            stream = null;
        }

        /// <inheritdoc />
        public void Revert() => throw new NotSupportedException();

        /// <inheritdoc />
        public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            long position = stream?.Seek(dlibMove, (SeekOrigin)dwOrigin)??0;

            // Dereference newPositionPtr and assign to the pointed location.
            if (plibNewPosition != IntPtr.Zero)
            {
                Marshal.WriteInt64(plibNewPosition, position);
            }
        }

        /// <inheritdoc />
        public void SetSize(long libNewSize) => throw new NotSupportedException();

        /// <inheritdoc />
        public void Stat(out STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new STATSTG()
            {
                cbSize = stream.Length,
                type = 2,
                grfMode = 0,

            };
        }

        /// <inheritdoc />
        public void UnlockRegion(long libOffset, long cb, int dwLockType) => 
            throw new NotSupportedException();

        /// <inheritdoc />
        public void Write(byte[] pv, int cb, IntPtr pcbWritten) => throw new NotSupportedException();
    }
}