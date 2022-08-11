using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.FileSystem.CopyWithProgress;
using Melville.INPC;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;

namespace Melville.Wpf.Samples.FileOperations;

public partial class MonitoredCopyViewModel
{

    [AutoNotify] private IFile? source;
    [AutoNotify] private IFile? destination;
    [AutoNotify] private bool isCopying;
    [AutoNotify] private double progress;
    private CancellationTokenSource? tokenSource;
    
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
        IsCopying = true;
        try
        {
            await destination.CopyFrom(source, tokenSource.Token, FileAttributes.Normal, ReportProgress);
        }
        finally
        {
            IsCopying = false;
        }
    }

    private CopyProgressResult ReportProgress(
        long totalfilesize, long totalbytestransferred, long streamsize, 
        long streambytestransferred, uint dwstreamnumber, CopyProgressCallbackReason dwcallbackreason, 
        IntPtr hsourcefile, IntPtr hdestinationfile, IntPtr lpdata)
    {
        Progress = ((double)totalbytestransferred) / totalfilesize;
        return CopyProgressResult.PROGRESS_CONTINUE;
    }
}