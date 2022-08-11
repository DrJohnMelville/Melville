using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.FileSystem.CopyWithProgress;
using Melville.INPC;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.WpfControls.FileDownloadBars;

namespace Melville.Wpf.Samples.FileOperations;

public partial class MonitoredCopyViewModel
{

    [AutoNotify] private IFile? source;
    [AutoNotify] private IFile? destination;
    [AutoNotify] private double progress;
    private CancellationTokenSource? tokenSource;

    [FromConstructor] public FileDownloadBarViewModel Downloader { get; }
    
    public void OpenSource([FromServices]IOpenSaveFile fileDlg)
    {
        if (fileDlg.GetLoadFile(null, "", "All files|*.*", "Select the source file") is { } file)
            Source = file;
    }
    public void OpenDest([FromServices]IOpenSaveFile fileDlg)
    {
        if (fileDlg.GetSaveFile(null, "", "All files|*.*", "Select the destination file") is { } file)
            Destination = file;
    }

    [AutoNotify] public bool CanCopy => Source is not null && Source.Exists() && Destination is not null;

    public async Task DoCopy()
    {
        tokenSource = new CancellationTokenSource();
        if (Source is not null && Destination is not null)
            await Downloader.CopyFile(
                Destination, Source, FileAttributes.Normal, tokenSource.Token);
    }

    public void DoCancel() => tokenSource?.Cancel();
}