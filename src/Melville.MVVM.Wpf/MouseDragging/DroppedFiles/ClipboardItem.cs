using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Memory;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

public readonly struct ClipboardItem(FORMATETC format, object data, bool release)
{
    public string FormatName => FormatNamer.NameForFormat(format.cfFormat);
    public void WriteFormat(ref FORMATETC target)
    {
        target = format;
    }

    public bool Matches(ref FORMATETC candidate)
    {
        return candidate.cfFormat == format.cfFormat &&
               ((candidate.dwAspect | format.dwAspect) is not 0) &&
               (candidate.lindex == format.lindex || format.lindex is -1 || candidate.lindex is -1) &&
               candidate.ptd == format.ptd &&
               ((candidate.tymed | format.tymed) is not 0);
    }

    public void ReturnValue(out STGMEDIUM medium)
    {
        switch (data)
        {
            case String str: WriteStringTo(str, out medium); break;
            case byte[] bytes: WriteBytesTo(bytes, out medium); break;
            case Stream stream: WriteStreamTo(stream, out medium); break;
            default: throw new NotImplementedException("cannot convert to hglobal");
        }
    }

    private void WriteStringTo(string str, out STGMEDIUM medium)
    {
        var handle = HGlobalUtils.CreateMovableHGlobal((str.Length + 1)*2, target =>
            {
                var bytes = MemoryMarshal.Cast<char, byte>(str.AsSpan());
                bytes.CopyTo(target);
                target[bytes.Length] = 0;
                target[bytes.Length + 1] = 0;
            });
        CreateStgMediumForHGlobal(handle, out medium);
    }

    private void WriteBytesTo(byte[] bytes, out STGMEDIUM medium)
    {
        var handle = HGlobalUtils.CreateMovableHGlobal(bytes.Length, target=>
            bytes.AsSpan().CopyTo(target));
        CreateStgMediumForHGlobal(handle, out medium);
    }

    private void WriteStreamTo(Stream data, out STGMEDIUM medium)
    {
        if (data.Length == 0) throw new NotSupportedException("streams must declare their length");
        data.Seek(0, SeekOrigin.Begin);
        var handle = HGlobalUtils.CreateMovableHGlobal((int)data.Length, data.ReadExactly);
        CreateStgMediumForHGlobal(handle, out medium);
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
}

public static class HGlobalUtils
{
    public static unsafe nint CreateMovableHGlobal(int size, Action<Span<byte>> fill)
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

public static class FormatNamer
{
    public static string NameForFormat(short format)
    {
        var buffer = new char[200];
        var name = PInvoke.GetClipboardFormatName((ushort)format, buffer);
        var asSpan = buffer.AsSpan();
        asSpan = asSpan.Slice(0, asSpan.IndexOf('\0'));
        return asSpan.ToString();
    }
}