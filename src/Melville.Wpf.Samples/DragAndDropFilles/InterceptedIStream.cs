using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Melville.Linq;

namespace Melville.Wpf.Samples.DragAndDropFilles;

internal class InterceptedIStream(IStream inner, DDInterceptorViewModel viewModel, int number): 
    IStream
{
    /// <inheritdoc />
    public void Clone(out IStream ppstm)
    {
        inner.Clone(out ppstm);
        viewModel.AddEvent($"{number} Clone", "No visualizer");
    }

    /// <inheritdoc />
    public void Commit(int grfCommitFlags)
    {
        inner.Commit(grfCommitFlags);
        viewModel.AddEvent($"{number} Commit", "No visualizer");
    }

    /// <inheritdoc />
    public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
    {
        inner.CopyTo(pstm, cb, pcbRead, pcbWritten);
        viewModel.AddEvent($"{number} CopyTo", $"""
        Requested size: {cb}
        Read: {Marshal.ReadInt32(pcbRead)}
        Written: {Marshal.ReadInt32(pcbWritten)}
        """);
    }

    /// <inheritdoc />
    public void LockRegion(long libOffset, long cb, int dwLockType)
    {
        inner.LockRegion(libOffset, cb, dwLockType);
        viewModel.AddEvent($"{number} LockRegion", "No visualizer");
    }

    /// <inheritdoc />
    public void Read(byte[] pv, int cb, IntPtr pcbRead)
    {
        inner.Read(pv, cb, pcbRead);
        viewModel.AddEvent($"{number} Read", 
            pv.Take(Marshal.ReadInt32(pcbRead)).BinaryFormat().ConcatenateStrings());
    }

    /// <inheritdoc />
    public void Revert()
    {
        inner.Revert();
        viewModel.AddEvent($"{number} Revert", "No visualizer");
    }

    /// <inheritdoc />
    public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
    {
        inner.Seek(dlibMove, dwOrigin, plibNewPosition);
        viewModel.AddEvent($"{number} Seek", $"""
        Requested Position: {dlibMove}
        dwOrigin: {dwOrigin}
        New position: {Marshal.ReadInt64(plibNewPosition)}
        """);
    }

    /// <inheritdoc />
    public void SetSize(long libNewSize)
    {
        inner.SetSize(libNewSize);
        viewModel.AddEvent($"{number} SetSize", $"""
        Requested size: {libNewSize}
        """);
    }

    /// <inheritdoc />
    public void Stat(out STATSTG pstatstg, int grfStatFlag)
    {
        inner.Stat(out pstatstg, grfStatFlag);
        viewModel.AddEvent($"{number} Stat", $"""
        Name: {pstatstg.pwcsName}
        Type: {pstatstg.type}
        Size: {pstatstg.cbSize}
        mTime : {pstatstg.mtime}
        cTime : {pstatstg.ctime}
        aTime : {pstatstg.atime}
        Mode: {pstatstg.grfMode}
        LocksSupported: {pstatstg.grfLocksSupported}
        clsid: {pstatstg.clsid}
        State: 0x{pstatstg.grfStateBits:X}
        Reserved: {pstatstg.reserved}
        """);
    }

    /// <inheritdoc />
    public void UnlockRegion(long libOffset, long cb, int dwLockType)
    {
        inner.UnlockRegion(libOffset, cb, dwLockType);
        viewModel.AddEvent($"{number} UnlockRegion", "No visualizer");
    }

    /// <inheritdoc />
    public void Write(byte[] pv, int cb, IntPtr pcbWritten)
    {
        inner.Write(pv, cb, pcbWritten);
        viewModel.AddEvent($"{number} Write",
            pv.Take(Marshal.ReadInt32(pcbWritten)).BinaryFormat().ConcatenateStrings());
    }
}