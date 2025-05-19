using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using static System.Windows.Forms.DataFormats;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

#warning -- needs to be disposed when we are done with it

internal class NativeConstants
{
    public const int E_NOTIMPL = -2147467263;
    public const int E_ADVISENOTSUPPORTED = -2147221501;
    public const int S_OK = 0;
    public const int S_FALSE = 1;
    public const int DATA_S_SAMEFORMATETC = unchecked((int)0x00040130);
    public const int DV_E_FORMATETC = -2147221404;
}

public class ComDataObject : System.Runtime.InteropServices.ComTypes.IDataObject
{
    public readonly List<ClipboardItem> items = new();

    /// <inheritdoc />
    public int DAdvise(ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection)
    {
        connection = 0;
        return NativeConstants.E_NOTIMPL;
    }

    /// <inheritdoc />
    public void DUnadvise(int connection)
    {
        Marshal.ThrowExceptionForHR(NativeConstants.E_NOTIMPL);
    }

    /// <inheritdoc />
    public int EnumDAdvise(out IEnumSTATDATA? enumAdvise)
    {
        enumAdvise = null;
        return NativeConstants.E_ADVISENOTSUPPORTED;
    }

    /// <inheritdoc />
    public IEnumFORMATETC EnumFormatEtc(DATADIR direction)
    {
        return direction == DATADIR.DATADIR_GET
            ? new ComFormatEnumerator(items)
            : throw new NotImplementedException("Only DATADIR_GET is supported");
    }

    /// <inheritdoc />
    public int GetCanonicalFormatEtc(ref FORMATETC formatIn, out FORMATETC formatOut)
    {
        formatOut = formatIn;
        return NativeConstants.DATA_S_SAMEFORMATETC;
    }

    /// <inheritdoc />
    public void GetData(ref FORMATETC format, out STGMEDIUM medium)
    {
        var items = CollectionsMarshal.AsSpan(this.items);
        for (int i = items.Length - 1; i >= 0; i--)
        {
            ref var item = ref items[i];
            if (item.Matches(ref format))
            {
                UdpConsole.WriteLine($"Found a {FormatNamer.NameForFormat(format.cfFormat)}");
                item.ReturnValue(out medium);
                return;
            }
        }

        UdpConsole.WriteLine($"Could not find a {FormatNamer.NameForFormat(format.cfFormat)}");

        if (FormatNamer.NameForFormat(format.cfFormat) == "FileGroupDescriptorW")
        {
            ;
        }

        medium = new STGMEDIUM()
        {
            tymed = TYMED.TYMED_NULL
        };
    }

    public static class UdpConsole
    {
        private static UdpClient? client = null;
        private static UdpClient Client
        {
            get
            {
                client ??= new UdpClient();
                return client;
            }
        }

        public static string WriteLine(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            Client.Send(bytes, bytes.Length, "127.0.0.1", 15321);
            return str;
        }
    }
    /// <inheritdoc />
    public void GetDataHere(ref FORMATETC format, ref STGMEDIUM medium)
    {
    }

    /// <inheritdoc />
    public int QueryGetData(ref FORMATETC format)
    {
        return 0;
    }

    /// <inheritdoc />
    public unsafe void SetData(ref FORMATETC formatIn, ref STGMEDIUM medium, bool release)
    {
        UdpConsole.WriteLine($"Writing a {FormatNamer.NameForFormat(formatIn.cfFormat)}");

        if (medium.tymed != TYMED.TYMED_HGLOBAL)
            throw new NotImplementedException("Expecting a hglobal");
        var handle = new HGLOBAL((IntPtr)medium.unionmember);
        var size = PInvoke.GlobalSize(handle);
        var ptr = PInvoke.GlobalLock(handle);
        try
        {
            var span = new Span<byte>(ptr, (int)size);
            var data = span.ToArray();
            this.SetData(formatIn, data);
        }
        finally
        {
            PInvoke.GlobalUnlock(handle);
            if (release) PInvoke.GlobalFree(handle);
#warning dispose of the incomming hglobal
        }
    }

    public void SetData(FORMATETC format, object item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Matches(ref format))
            {
                items[i] = new ClipboardItem(format, item, true);
                return;
            }
        }
        items.Add(new ClipboardItem(format, item, true));
    }
}

public static class ComDataObjectExtensions
{
    public static void SetText(this ComDataObject target, string text, int index = -1) =>
        target.SetData(CreateFormatEtc(DataFormats.UnicodeText, index),
            text);

    private static FORMATETC CreateFormatEtc(string format, int index) =>
        new()
        {
            cfFormat = (short)DataFormats.GetFormat(format).Id,
            dwAspect = DVASPECT.DVASPECT_CONTENT,
            lindex = index,
            ptd = IntPtr.Zero,
            tymed = TYMED.TYMED_HGLOBAL
        };

    public static void SetData(
        this ComDataObject target, string format, Stream data, int index = -1)
    {
        target.SetData(CreateFormatEtc(format, index), data);
    }

    public static void SetData(
        this ComDataObject target, string format, byte[] data, int index = -1)
    {
        target.SetData(CreateFormatEtc(format, index), data);
    }
}