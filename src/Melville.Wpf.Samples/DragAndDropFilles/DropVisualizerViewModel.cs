using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows;
using Melville.Hacks.Reflection;
using Melville.INPC;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MouseClicks;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Melville.MVVM.Wpf.MouseDragging.DroppedFiles;
using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace Melville.Wpf.Samples.DragAndDropFilles;

public partial class DropVisualizerViewModel()
{
    [AutoNotify] public partial IReadOnlyList<DataFormatViewModel>? Items { get; set; }
    public async Task DropItems(IDropAction drop)
    {
        if (Unwrap(drop.Item) is not {} innerDo)
            return;
        var ret = new List<DataFormatViewModel>();
        foreach (var format in innerDo.EnumFormatEtc(DATADIR.DATADIR_GET).Wrap())
        {
            var model = new DataFormatViewModel(format);
            model.CaptureData(innerDo);
            ret.Add(model);
        }

        Items = ret;
    }

    private IDataObject Unwrap(object? o) => o switch
    {
        DataObject => Unwrap(o.GetField("_innerData")),
        _ when o.GetType().Name.Contains("OleConverter") => Unwrap(o.GetField("_innerData")),
        IDataObject d => d,
        _ => null
    };

    public DragDropEffects DropItems(IDropQuery query)
    {
        query.AdornTarget(DropAdornerKind.Rectangle);
        return DragDropEffects.Copy;
    }

    public void StartDrag(IMouseClickReport mcr, [FromServices] DDInterceptor interceptor)
    {
        if (mcr.ClickCount() > 1) return;
        mcr.DragSource().DragTarget(0.5)
            .Drag(interceptor.BeginIntercept(DataToDrag2()), DragDropEffects.Copy,
                _=>interceptor.ShowTrace());
    }

    private DataObject DataToDrag2()
    {
        var ret = new ComDataObject();
        foreach (var item in Items.Where(i=>i.Included))
        {
            item.AddTo(ret);
        }
        return new DataObject(ret);
    }

    public void StartClipboardDrag(IMouseClickReport mcr, [FromServices]DDInterceptor interceptor)
    {
        if (mcr.ClickCount() > 1 || Clipboard.GetDataObject() is not {} clip) return;
        mcr.DragSource().DragTarget(0.5)
            .Drag(interceptor.BeginIntercept(clip), DragDropEffects.Copy, 
                _=> interceptor.ShowTrace());
    }

}

public partial class DataFormatViewModel
{
    [AutoNotify] public partial bool Included { get; set; } //# = true;
    [FromConstructor] private readonly FORMATETC format;
    public string Kind => DataFormats.GetDataFormat(format.cfFormat).Name;
    public int Index => format.lindex;
    public String Type => format.tymed.ToString();
    public string Aspect => format.dwAspect.ToString();
    public int Objects => data.Count(i=>i is not null);
    private readonly List<byte[]> data = new();
    private int baseIndex = -1;

    public void CaptureData(IDataObject innerDo)
    {
        if (ReadSingleDatum(innerDo, -1) is { } item)
        {
            data.Add(item);
            return;
        }
        baseIndex = 0;
        for (int i = 0; ReadSingleDatum(innerDo, i) is {} loopItem; i++)
        {
            data.Add(loopItem);
        }
    }

    private byte[]? ReadSingleDatum(IDataObject innerDo, int index)
    {
        try
        {
            var fmt = format with { tymed = SelectFormat(), lindex = index};
            if (innerDo.QueryGetData(ref fmt) is not 0) return null;
            innerDo.GetData(ref fmt, out var medium);
            return medium.ConsumeToByteArray();
        }
        catch (Exception )
        {
            return null;
        }
    }

    private TYMED SelectFormat()
    {
        if (format.tymed.HasFlag(TYMED.TYMED_HGLOBAL))
            return TYMED.TYMED_HGLOBAL;
        if (format.tymed.HasFlag(TYMED.TYMED_ISTREAM))
            return TYMED.TYMED_ISTREAM;
        throw new NotImplementedException("Cannot download the data");  
    }

    public void AddTo(ComDataObject ret)
    {
        for (int i = 0; i < data.Count; i++)
        {
            var finalFmt = format with { lindex = i + baseIndex };
            ret.SetData(in finalFmt, data[i]);
        }
    }
}