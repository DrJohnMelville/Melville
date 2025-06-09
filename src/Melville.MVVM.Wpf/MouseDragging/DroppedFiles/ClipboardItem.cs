using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Memory;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

public readonly struct ClipboardItem(FORMATETC format, object data)
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

    public bool IsComCompatible()=> data is String or Stream or byte[];

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

    public void WriteToHere(ref STGMEDIUM medium)
    {
        switch (medium.tymed)
        {
            case TYMED.TYMED_HGLOBAL:
                WriteToHGlobal(medium.unionmember);
                break;
            case TYMED.TYMED_ISTREAM:
                WriteToIStream(medium.unionmember);
                break;
            default:
                throw new NotSupportedException("Only HGlobal and Streams are supprted.");
        }
    }

    private unsafe void WriteToHGlobal(IntPtr targetHandle)
    {
        var handle = new HGLOBAL(targetHandle);
        var ptr = PInvoke.GlobalLock(handle);
        var size = PInvoke.GlobalSize(handle);
        var buffer = new Span<byte>(ptr, (int)size);
        switch (data)
        {
            case string s: WriteBytesTo(buffer, MemoryMarshal.Cast<char,byte>(s.AsSpan())); break;
            case byte[] b: WriteBytesTo(buffer, b.AsSpan()); break;
            case Stream s: s.ReadAtLeast(buffer, buffer.Length, false); break;
            default: throw new NotSupportedException("Only string, byte[] and stream are supported");
        }

        PInvoke.GlobalUnlock(handle);
    }

    private void WriteBytesTo(Span<byte> buffer, ReadOnlySpan<byte> data) => 
        data[..Math.Min(buffer.Length, data.Length)].CopyTo(buffer);

    private void WriteToIStream(IntPtr target)
    {
        var stream = (IStream)Marshal.GetObjectForIUnknown(target);
        switch (data)
        {
            case string s: WriteBytesTo(stream, MemoryMarshal.Cast<char, byte>(s.AsSpan()).ToArray()); break;
            case byte[] b: WriteBytesTo(stream, b); break;
            case Stream s: WriteBytesTo(stream, s); break;
            default: throw new NotSupportedException("Only string, byte[] and stream are supported");
        }
    }

    private void WriteBytesTo(IStream stream, Stream source)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(4096);
        while (source.Read(buffer.AsSpan()) is var read and > 0)
        {
            stream.Write(buffer, read, IntPtr.Zero);
        }
    }

    private void WriteBytesTo(IStream stream, byte[] data) => stream.Write(data, data.Length, IntPtr.Zero);

    public object? DotNetValue() => new DotnetClipboardFormatter(format.cfFormat, data).ToDotNet();
}

public readonly struct DotnetClipboardFormatter(short format, object? data)
{
    private const short cfText = 1;
    private const short cfUnicodeText = 13;
    private const short cfOemText = 7;

    public object? ToDotNet() => (format, data) switch
    {
        (_, null) => null,
        (cfText or cfUnicodeText or cfOemText, String s) => s,
        (cfText or cfUnicodeText or cfOemText, Stream s) => 
            new DotnetClipboardFormatter(format,BytesFrom(s)).ToDotNet(),
        (cfText or cfOemText, byte[] data) => Encoding.UTF8.GetString(data),
        (cfUnicodeText, byte[] data) => Encoding.Unicode.GetString(data),
        (_, byte[] dataArray) => new MemoryStream(dataArray), // for compatibility with WPF implementation
        _=> data
    };

    private byte[] BytesFrom(Stream stream) => stream switch
    {
        MemoryStream ms => ms.ToArray(),
        {Length: > 0} => ReadToBuffer(stream),
        _=> CopyToBuffer(stream)
    };

    private byte[] ReadToBuffer(Stream stream)
    {
        var ret = new byte[stream.Length];
        stream.ReadExactly(ret.AsSpan());
        return ret;
    }

    private byte[] CopyToBuffer(Stream stream)
    {
        var ret = new MemoryStream();
        stream.CopyTo(ret);
        return ret.ToArray();
    }
}