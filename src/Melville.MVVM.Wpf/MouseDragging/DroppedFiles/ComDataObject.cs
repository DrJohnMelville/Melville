using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using IComDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;
using IWinDataObject = System.Windows.IDataObject;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

public class ComDataObject : IComDataObject, IWinDataObject
{
    public readonly List<ClipboardItem> items = new();

    #region IWinDDataObject

    /// <inheritdoc />
    public object? GetData(string format) => 
        ItemsMatching(format).FirstOrDefault().DotNetValue();

    /// <inheritdoc />
    public object? GetData(string format, bool autoConvert) => GetData(format);
    
    /// <inheritdoc />
    public object? GetData(Type format) =>
        format.ToString() is {Length:>0} fmtName? GetData(fmtName) : null;

    /// <inheritdoc />
    public bool GetDataPresent(string format) => ItemsMatching(format).Any();

    private IEnumerable<ClipboardItem> ItemsMatching(string format)
    {
        var formatAsInt = (short)DataFormats.GetDataFormat(format).Id;
        return items.Where(i => i.Format() == formatAsInt);
    }

    /// <inheritdoc />
    public bool GetDataPresent(string format, bool autoConvert) => GetDataPresent(format);

    /// <inheritdoc />
    public bool GetDataPresent(Type format) =>
        format.ToString() is { Length: > 0 } fmtName && GetDataPresent(fmtName);
    
    /// <inheritdoc />
    public string[] GetFormats() =>
        items.Select(i => i.Format())
            .Distinct()
            .Select(i => DataFormats.GetDataFormat(i).Name)
            .ToArray();

    /// <inheritdoc />
    public string[] GetFormats(bool autoConvert) => GetFormats();

    /// <inheritdoc />
    public void SetData(object data) => SetData(data.GetType(), data);
    
    /// <inheritdoc />
    public void SetData(string format, object data) => SetComData(format, data, -1);

    /// <inheritdoc />
    public void SetData(string format, object data, bool autoConvert) => SetData(format, data);
    
    /// <inheritdoc />
    public void SetData(Type format, object data)
    {
        if (format.ToString() is {Length: >0} formatName)
            SetData(formatName, data);
    }

    #endregion

    #region IComDataObject
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
    #endregion

    public  void SetText(string text, int index = -1) =>
        SetData(CreateFormatEtc(DataFormats.UnicodeText, index, TYMED.TYMED_HGLOBAL),
            text);

    private static FORMATETC CreateFormatEtc(string format, int index, TYMED type) =>
        new()
        {
            cfFormat = (short)DataFormats.GetDataFormat(format).Id,
            dwAspect = DVASPECT.DVASPECT_CONTENT,
            lindex = index,
            ptd = IntPtr.Zero,
            tymed = type
        };

    public void SetComData(string format, object data, int index = -1) => 
        SetData(CreateFormatEtc(format, index, TYMED.TYMED_ISTREAM | TYMED.TYMED_HGLOBAL), data);
}