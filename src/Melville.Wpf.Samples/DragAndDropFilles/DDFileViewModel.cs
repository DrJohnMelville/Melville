using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Melville.FileSystem;
using Melville.INPC;
using Melville.MVVM.Wpf.MouseClicks;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

namespace Melville.Wpf.Samples.DragAndDropFilles;

public partial class DDFileViewModel
{
    [AutoNotify] public partial string DroppedFileName { get; set; } //# = "<None>";
    [AutoNotify] public partial string DroppedFileContent { get; set; } //# = "<None>";

    public async Task DropItems(IDropAction drop)
    {
        var diskConnector = new DiskFileSystemConnector();
            foreach (var file in drop.Item.GetDroppedFiles(diskConnector.FileFromPath))
            {
                DroppedFileName += file.Name + ", ";
                var buffer = new byte[1000];
                await using var stream = await file.OpenRead();
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                DroppedFileContent += Encoding.UTF8.GetString(buffer, 0, bytesRead) + "\r\n----\r\n";
            }
    }


    public DragDropEffects DropItems(IDropQuery query)
    {
        if (!ShouldAcceptDrop(query.Item)) return DragDropEffects.None;
        query.AdornTarget(DropAdornerKind.Rectangle);
        return DragDropEffects.Copy;
    }

    private static readonly string[] DropFormats = new[]
    {
        DataFormats.FileDrop, DataFormats.Text,
        StreamingFileClipboardFormats.NarrowGroup,
        StreamingFileClipboardFormats.WideGroup
    };
    private bool ShouldAcceptDrop(IDataObject queryItem) =>
        queryItem.GetFormats().Any(DropFormats.Contains);

    public void Drag(IMouseClickReport mcr)
    {
        DroppedFileContent = DroppedFileName = "";
        if (mcr.ClickCount() > 1) return;
        mcr.DragSource().DragTarget(0.5)
            .Drag(DataToDrag, DragDropEffects.Copy);
    }

    private IDataObject DataToDrag()
    {
        var ret = new ComDataObject();
        ret.PushStreams(        
            ("File1.txt", new MemoryStream("File 1 data"u8.ToArray())));
        return ret;
    }

    public void Drag2(IMouseClickReport mcr)
    {
        DroppedFileContent = DroppedFileName = "";
        if (mcr.ClickCount() > 1) return;
        mcr.DragSource().DragTarget(0.5)
            .Drag(DataToDrag2, DragDropEffects.Copy);
    }

    private IDataObject DataToDrag2()
    {
        var ret = new ComDataObject();
        ret.PushStreams(
            ("File1.txt", new MemoryStream("File 1 data"u8.ToArray())),
            ("File2.txt", new MemoryStream("File 2 data"u8.ToArray())));
        return ret;
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_innerData")]
    private extern static ref IDataObject GetInnerDO(DataObject outer);
}