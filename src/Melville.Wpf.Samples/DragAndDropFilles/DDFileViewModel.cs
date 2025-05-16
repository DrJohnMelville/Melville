using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Melville.FileSystem;
using Melville.INPC;
using Melville.MVVM.WaitingServices;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
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
                DroppedFileName = file.Name;
                var buffer = new byte[1000];
                await using var stream = await file.OpenRead();
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                DroppedFileContent = Encoding.UTF8.GetString(buffer, 0, bytesRead);
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
        DataObjectReaderExtensions.NarrowFileGroupDescriptorFormat,
        DataObjectReaderExtensions.WideFileGroupDescriptorFormat
    };
    private bool ShouldAcceptDrop(IDataObject queryItem) =>
        queryItem.GetFormats().Any(DropFormats.Contains);

}