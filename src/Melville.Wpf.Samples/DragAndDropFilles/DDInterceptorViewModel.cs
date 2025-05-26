using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

    [ComImport()]
    [Guid("00020400-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IDispatch
    {
        [PreserveSig]
        int GetTypeInfoCount(out int Count);

        [PreserveSig]
        int GetTypeInfo
        (
            [MarshalAs(UnmanagedType.U4)] int iTInfo,
            [MarshalAs(UnmanagedType.U4)] int lcid,
            out System.Runtime.InteropServices.ComTypes.ITypeInfo typeInfo
        );

        [PreserveSig]
        int GetIDsOfNames
        (
            ref Guid riid,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)]
            string[] rgsNames,
            int cNames,
            int lcid,
            [MarshalAs(UnmanagedType.LPArray)] int[] rgDispId
        );

        [PreserveSig]
        int Invoke
        (
            int dispIdMember,
            ref Guid riid,
            uint lcid,
            ushort wFlags,
            ref System.Runtime.InteropServices.ComTypes.DISPPARAMS pDispParams,
            out object pVarResult,
            ref System.Runtime.InteropServices.ComTypes.EXCEPINFO pExcepInfo,
            out UInt32 pArgErr
        );
    }

    private int streamCounter;
    /// <inheritdoc />
    public void GetData(ref FORMATETC format, out STGMEDIUM medium)
    {
        medium = default;
        try
        {
            comDataObject.GetData(ref format, out medium);
        }
        catch (Exception e)
        {
            target.AddEvent("GetData Exception", $"""
            Request
            {DumpObject(format)}
            
            Result
            {DumpObject(medium)}
            
            Exception: 
            {e}
            """);
            throw;
        }

        
        if (medium.tymed == TYMED.TYMED_ISTREAM)
        {
            var stream = (IStream)Marshal.GetObjectForIUnknown(medium.unionmember);
            Marshal.Release(medium.unionmember);
        
            var wrapper = new InterceptedIStream(stream, target, streamCounter++);
            var handle = Marshal.GetIUnknownForObject(wrapper);

            medium = medium with { unionmember = handle };
        }

        target.AddEvent($"GetData {FormatString(format.cfFormat)}", $"""
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
        target.AddEvent($"QueryGetData 0X{ret:X8}", DumpObject(format));
        return ret;
    }

    /// <inheritdoc />
    public void SetData(ref FORMATETC formatIn, ref STGMEDIUM medium, bool release)
    {
        target.AddEvent($"SetData {FormatString(formatIn.cfFormat)}", $"""
            Format
            {DumpObject(formatIn)}
            
            Data
            {DumpObject(medium)}
            
            Release = {release}
            """);
        comDataObject.SetData(ref formatIn, ref medium, release);
    }

    private unsafe string DumpObject(object o) => o switch
    {
         FORMATETC format =>  $"""
            Format: {FormatString(format.cfFormat)}
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

    private string FormatString(short fmt) =>
        $"{DataFormats.GetDataFormat(fmt).Name} (0x{fmt:X4})";

    private string DumpMedium(STGMEDIUM med) =>
        med.tymed switch
        {
            TYMED.TYMED_HGLOBAL =>
                String.Join(Environment.NewLine, med.ToByteArray().BinaryFormat()),
            // TYMED.TYMED_ISTREAM =>
            //     String.Join(Environment.NewLine, med.ToByteArray().BinaryFormat()),
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

