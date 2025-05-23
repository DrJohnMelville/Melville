using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Documents;
using Melville.Hacks.Reflection;
using Melville.INPC;
using Melville.Linq;
using Melville.Lists;
using Melville.MVVM.Wpf.MouseDragging.DroppedFiles;
using Melville.MVVM.Wpf.MvvmDialogs;
using IComDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;
using IDataObject = System.Windows.IDataObject;

namespace Melville.Wpf.Samples.DragAndDropFilles;

public class DDInterceptor (IMvvmDialog dialog)
{
    DDInterceptorViewModel model = new();

    public IDataObject BeginIntercept(IDataObject source)
    {
        return new InterceptionShim(source, model).Rewrapped();
    }

    public void ShowTrace()
    {
        dialog.ShowPopupWindow(model, 800, 600, "Drag Drop Interceptor");
    }
}



internal class InterceptionShim(
    IDataObject inner, 
    DDInterceptorViewModel target) : IComDataObject
{
    private IComDataObject comDataObject = Unwrap(inner);

    private static IComDataObject Unwrap(object? o) => o switch
    {
        DataObject => Unwrap(o.GetField("_innerData")),
        _ when o.GetType().Name.Contains("OleConverter") => Unwrap(o.GetField("_innerData")),
        IComDataObject d => d,
        _ => null
    };

    public IDataObject Rewrapped() => new DataObject(this);

    /// <inheritdoc />
    public int DAdvise(ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection)
    {
        target.AddEvent("DAdvise", "No Visualizer");
        return comDataObject.DAdvise(ref pFormatetc, advf, adviseSink, out connection);
    }

    /// <inheritdoc />
    public void DUnadvise(int connection)
    {
        target.AddEvent("DUnAdvise", "No visualizer");
        comDataObject.DUnadvise(connection);
    }

    /// <inheritdoc />
    public int EnumDAdvise(out IEnumSTATDATA? enumAdvise)
    {
        target.AddEvent("EnumDAdvise", "No visualizer");
        return comDataObject.EnumDAdvise(out enumAdvise);
    }

    /// <inheritdoc />
    public IEnumFORMATETC EnumFormatEtc(DATADIR direction)
    {
        var ret = comDataObject.EnumFormatEtc(direction);
        ret.Clone(out var clone);
        target.AddEvent("EnumFormatEtc", 
            string.Join("\r\n-----------------\r\n", clone.Wrap().Select(i=>DumpObject(i))));
        return ret;
    }

    /// <inheritdoc />
    public int GetCanonicalFormatEtc(ref FORMATETC formatIn, out FORMATETC formatOut)
    {
        var ret = comDataObject.GetCanonicalFormatEtc(ref formatIn, out formatOut);
        target.AddEvent("GetCannonicalFormatEtc", $"""
            Input
            {DumpObject(formatIn)}

            Output = {ret}
            {DumpObject(formatOut)}
            """);
        return ret;
    }

    /// <inheritdoc />
    public void GetData(ref FORMATETC format, out STGMEDIUM medium)
    {
        comDataObject.GetData(ref format, out medium);
        target.AddEvent("GetData", $"""
            Request
            {DumpObject(format)}
            
            Result
            {DumpObject(medium)}
            """);
    }

    /// <inheritdoc />
    public void GetDataHere(ref FORMATETC format, ref STGMEDIUM medium)
    {
        comDataObject.GetDataHere(ref format, ref medium);
        target.AddEvent("GetDataHere", $"""
            Request
            {DumpObject(format)}
            
            Result
            {DumpObject(medium)}
            """);
    }

    /// <inheritdoc />
    public int QueryGetData(ref FORMATETC format)
    {
        var ret = comDataObject.QueryGetData(ref format);
        target.AddEvent($"QueryGetData {ret}", DumpObject(format));
        return ret;
    }

    /// <inheritdoc />
    public void SetData(ref FORMATETC formatIn, ref STGMEDIUM medium, bool release)
    {
        comDataObject.SetData(ref formatIn, ref medium, release);
        target.AddEvent("SetData", $"""
            Format
            {DumpObject(formatIn)}
            
            Data
            {DumpObject(medium)}
            
            Release = {release}
            """);
    }

    private unsafe string DumpObject(object o) => o switch
    {
         FORMATETC format =>  $"""
            Format: {DataFormats.GetDataFormat(format.cfFormat).Name} (0x{format.cfFormat:X4})
            Index: {format.lindex}
            Ptd: 0x{format.ptd:X16}
            Aspect: {format.dwAspect}
            Medium Type: {format.tymed}
         """,
         STGMEDIUM med => $"""
            Medium Type: {med.tymed},
            UnionMember: {med.unionmember:X16}
            pUnkForRelease: {(nint)(&med.pUnkForRelease):X16}
            {DumpMedium(med)}
         """,
        _ => o.ToString()?? ""
    };

    private string DumpMedium(STGMEDIUM med) =>
        med.tymed switch
        {
            TYMED.TYMED_HGLOBAL =>
                String.Join(Environment.NewLine, med.ToByteArray().BinaryFormat()),
            _ => "<Cannot Extract Data>"
        };
}

public partial class DDInterceptorViewModel
{
    public IList<DDEvent> Events { get; } =
        new List<DDEvent>();

    public record DDEvent(string Title, string Data)
    {
    }


    public void AddEvent(string title, string data)
    {
        Events.Add(new DDEvent(title, data));
    }
}

