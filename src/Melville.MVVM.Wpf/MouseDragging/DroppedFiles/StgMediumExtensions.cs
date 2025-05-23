using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

public static class StgMediumExtensions
{
    public static byte[] ConsumeToByteArray(this ref STGMEDIUM medium)
    {
        try
        {
            return ToByteArray(medium);
        }
        finally
        {
            ReleaseStgMedium(ref medium);
        }
    }

    [DllImport("OLE32.dll", ExactSpelling = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [SupportedOSPlatform("windows5.0")]
    internal static extern void ReleaseStgMedium(ref STGMEDIUM param0);


    public static byte[] ToByteArray(this in STGMEDIUM medium)
    {
        return medium.tymed switch
        {
            var i when i.HasFlag(TYMED.TYMED_HGLOBAL) => ReadHGlobal(medium),
            var i when i.HasFlag(TYMED.TYMED_ISTREAM) => ReadStream(medium),
            _ => throw new NotImplementedException("TyMED not implemented")
        };
    }


    private static unsafe byte[] ReadHGlobal(STGMEDIUM medium)
    {
        var handle = new HGLOBAL((IntPtr)medium.unionmember);
        var size = PInvoke.GlobalSize(handle);
        var ptr = PInvoke.GlobalLock(handle);
        try
        {
            return new Span<byte>(ptr, (int)size).ToArray();
        }
        finally
        {
            PInvoke.GlobalUnlock(handle);
        }
    }
    private static byte[] ReadStream(in STGMEDIUM medium)
    {
        using var stream = medium.ExtractFileStream();
        var ret = new byte[stream.Length];
        stream.ReadExactly(ret, 0, ret.Length);
        return ret;
    }
}