﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Printing.IndexedProperties;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

internal class NativeConstants
{
    public const int E_NOTIMPL = -2147467263;
    public const int E_ADVISENOTSUPPORTED = -2147221501;
    public const int S_OK = 0;
    public const int S_FALSE = 1;
    public const int DATA_S_SAMEFORMATETC = unchecked((int)0x00040130);
    public const int DV_E_FORMATETC = unchecked((int)0x80040064);
    public const int DV_E_CLIPFORMAT = unchecked((int)0x8004006A);
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
    public IEnumFORMATETC EnumFormatEtc(DATADIR direction) =>
        direction == DATADIR.DATADIR_GET
            ? new ComFormatEnumerator(items.DistinctBy(i=>i.Format()).ToArray())
            : throw new NotImplementedException("Only DATADIR_GET is supported");

    /// <inheritdoc />
    public int GetCanonicalFormatEtc(ref FORMATETC formatIn, out FORMATETC formatOut)
    {
        formatOut = formatIn;
        return NativeConstants.DATA_S_SAMEFORMATETC;
    }

    /// <inheritdoc />
    public void GetData(ref FORMATETC format, out STGMEDIUM medium)
    {
        if (FindItem(in format) is {} item)
        {
            item.ReturnValue(in format, out medium);
        }
        else
        {

            medium = new STGMEDIUM()
            {
                tymed = TYMED.TYMED_NULL
            };
        }
    }

    private ClipboardItem? FindItem(in FORMATETC format)
    {
        var items = CollectionsMarshal.AsSpan(this.items);
        foreach (var item in items)
        {
            if (item.Matches(format)) return item;
        }

        return null;
    }

    /// <inheritdoc />
    public void GetDataHere(ref FORMATETC format, ref STGMEDIUM medium)
    {
        if (FindItem(in format) is not { } item) return;
        item.WriteToHere(ref medium);
    }

    /// <inheritdoc />
    public int QueryGetData(ref FORMATETC format) => 
        FindItem(in format) is null ? NativeConstants.DV_E_CLIPFORMAT : NativeConstants.S_OK;

    /// <inheritdoc />
    public void SetData(ref FORMATETC formatIn, ref STGMEDIUM medium, bool release)
    {
        SetData(formatIn, medium.ConsumeToByteArray(release));
    }

    public void SetData(in FORMATETC format, object item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Matches(in format))
            {
                items[i] = new ClipboardItem(format, item);
                return;
            }
        }
        items.Add(new ClipboardItem(format, item));
    }
}

public static class ComDataObjectExtensions
{
    public static void SetText(this ComDataObject target, string text, int index = -1) =>
        target.SetData(CreateFormatEtc(DataFormats.UnicodeText, index, TYMED.TYMED_HGLOBAL),
            text);

    private static FORMATETC CreateFormatEtc(string format, int index, TYMED type) =>
        new()
        {
            cfFormat = (short)DataFormats.GetFormat(format).Id,
            dwAspect = DVASPECT.DVASPECT_CONTENT,
            lindex = index,
            ptd = IntPtr.Zero,
            tymed = type
        };

    public static void SetData(
        this ComDataObject target, string format, Stream data, int index = -1)
    {
        target.SetData(CreateFormatEtc(format, index, TYMED.TYMED_HGLOBAL), data);
    }

    public static void SetData(
        this ComDataObject target, string format, byte[] data, int index = -1)
    {
        target.SetData(CreateFormatEtc(format, index, TYMED.TYMED_HGLOBAL), data);
    }
}