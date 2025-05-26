using System;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Memory;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

public readonly struct ClipboardItem(FORMATETC format, object data, bool release)
{
    public void WriteFormat(ref FORMATETC target)
    {
        target = format;
        target.lindex = -1;
    }

    public short Format() => format.cfFormat;

    public bool Matches(in FORMATETC candidate)
    {
        return candidate.cfFormat == format.cfFormat &&
               ((candidate.dwAspect | format.dwAspect) is not 0) &&
               (candidate.lindex == format.lindex || format.lindex is -1 || candidate.lindex is -1);
    }

    public void ReturnValue(in FORMATETC formatetc, out STGMEDIUM medium)
    {
        switch (data)
        {
            case String str: WriteStringTo(str, out medium); break;
            case byte[] bytes: WriteBytesTo(bytes, formatetc.tymed, out medium); break;
            case Stream stream: WriteStreamTo(stream, formatetc.tymed, out medium); break;
            default: throw new NotImplementedException("cannot convert to hglobal");
        }
    }

    private void WriteStringTo(string str, out STGMEDIUM medium)
    {
        var handle = CreateMovableHGlobal((str.Length + 1)*2, target =>
            {
                var bytes = MemoryMarshal.Cast<char, byte>(str.AsSpan());
                bytes.CopyTo(target);
                target[bytes.Length] = 0;
                target[bytes.Length + 1] = 0;
            });
        CreateStgMediumForHGlobal(handle, out medium);
    }

    private void WriteBytesTo(byte[] bytes, TYMED tymed, out STGMEDIUM medium)
    {
        switch (tymed)
        {
            case var _ when tymed.HasFlag(TYMED.TYMED_HGLOBAL):
                WriteAsHGlobal(bytes, out medium);
                return;
            case var _ when tymed.HasFlag(TYMED.TYMED_ISTREAM):
                WriteAsStream(new MemoryStream(bytes), out medium);
                return;
            default:
                throw new NotImplementedException("Cannot render in an acceptable type");
        }
    }

    private static void WriteAsHGlobal(byte[] bytes, out STGMEDIUM medium)
    {
        var handle = CreateMovableHGlobal(bytes.Length, target=>
            bytes.AsSpan().CopyTo(target));
        CreateStgMediumForHGlobal(handle, out medium);
    }

    private void WriteStreamTo(Stream data, TYMED tymed, out STGMEDIUM medium)
    {
        switch (tymed)
        {
            case var _ when tymed.HasFlag(TYMED.TYMED_ISTREAM):
                WriteAsStream(data, out medium);
                return;
            case var _ when tymed.HasFlag(TYMED.TYMED_HGLOBAL):
                WriteAsHGlobal(data, out medium);
                return;
            default:
                throw new NotImplementedException("Cannot render in an acceptable type");
        }
    }

    private static void WriteAsStream(Stream data, out STGMEDIUM medium)
    {
        var wrapper = data.WrapWithComIStream();
        var handle = Marshal.GetIUnknownForObject(wrapper);
        medium = new()
        {
            tymed = TYMED.TYMED_ISTREAM,
            unionmember = handle,
            pUnkForRelease = null
        };
    }

    private void WriteAsHGlobal(Stream stream, out STGMEDIUM medium)
    {
        if (stream.Length is 0)
        {
            var temp = new MemoryStream();
            stream.CopyTo(temp);
            stream.Dispose();
            WriteAsHGlobal(temp.ToArray(), out medium);
            return;
        }
        var bytes = new byte[stream.Length];
        stream.ReadExactly(bytes.AsSpan());
        WriteAsHGlobal(bytes, out medium);
    }

    private static void CreateStgMediumForHGlobal(nint global, out STGMEDIUM medium)
    {
        medium = new STGMEDIUM()
        {
            tymed = TYMED.TYMED_HGLOBAL,
            unionmember = global,
            pUnkForRelease = null
        };
    }
    
    private static unsafe nint CreateMovableHGlobal(int size, Action<Span<byte>> fill)
    {
        var handle = PInvoke.GlobalAlloc(GLOBAL_ALLOC_FLAGS.GMEM_MOVEABLE, (uint)size);
        var ptr = PInvoke.GlobalLock(handle);
        try
        {
            var span = new Span<byte>(ptr, size);
            fill(span);
        }
        finally
        {
            PInvoke.GlobalUnlock(handle);
        }
        return handle;
    }
}