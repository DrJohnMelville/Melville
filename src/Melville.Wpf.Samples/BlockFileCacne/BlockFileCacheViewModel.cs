using Melville.FileSystem;
using Melville.FileSystem.BlockFile.BlockMultiStreams;
using Melville.FileSystem.BlockFile.ByteSinks;
using Melville.FileSystem.BlockFile.FileSystemObjects;
using Melville.INPC;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.SystemInterface.Time;
using Melville.WpfControls.FileDownloadBars;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.Wpf.Samples.BlockFileCacne;

public partial class BlockFileCacheViewModel
{
    public FileDownloadBarViewModel DownloadBarViewModel { get; } = new(
        new DownloadSpeedComputer(new WallClock()));

    [AutoNotify]
    public partial string FileName { get; set; } //# = "No File Loaded";

    public async Task LoadFile([FromServices]IOpenSaveFile osf)
    {
        var file = osf.GetLoadFile(null, "*.pdd", "Pdd Files (*.pdd)|*.pdd",
            "Select a PDD file to load");
        if (file is null) return;
        await Task.Run(()=>LoadFile(file));
    }

    private async Task LoadFile(IFile file)
    {
        var store = await BlockMultiStream.CreateFrom(new MemoryMappedByteSink(file.Path));
  //      var store = await BlockMultiStream.CreateFrom(new FileByteSink(file.Path));
        var root = new BlockRootDirectory(store);
        await root.ReadFromStore();
        var photos = root.SubDirectory("Photos");
        foreach (var f in photos.AllFiles())
        {
            FileName = f.Name;
            await Task.Delay(100);
            await DownloadBarViewModel.CopyFile(new MemoryFile("ljgbhhj"), f,
                FileAttributes.None, CancellationToken.None);
        }
    }
}
