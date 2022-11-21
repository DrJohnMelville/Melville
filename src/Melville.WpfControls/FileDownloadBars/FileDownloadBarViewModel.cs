using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Melville.FileSystem;
using Melville.FileSystem.CopyWithProgress;
using Melville.INPC;
using OperationCanceledException = System.OperationCanceledException;

namespace Melville.WpfControls.FileDownloadBars;

public partial class FileDownloadBarViewModel
{
    [AutoNotify] private Brush barBrush = Brushes.Blue;
    [AutoNotify] private Brush nonBarBrush = Brushes.White;
    [AutoNotify] private string message = "";
    [AutoNotify] private bool copying;
    [AutoNotify] private double offset;

    [FromConstructor] private readonly DownloadSpeedComputer speedComputer; 

    public async Task CopyFile(IFile destination, IFile source, FileAttributes attrs, CancellationToken token)
    {
        Copying = true;
        Message = "Searching...";
        speedComputer.ReportInterval(0);
        try
        {
            await destination.CopyFrom(source, token, attrs, ProgressFunc);
            Message = "Done";
        }
        catch (OperationCanceledException)
        {
            Message = "Cancelled";
        }
        Copying = false;
    }
    
    private CopyProgressResult ProgressFunc(
        long totalfilesize, long totalbytestransferred, long streamsize, long streambytestransferred,
        uint dwstreamnumber, CopyProgressCallbackReason dwcallbackreason, IntPtr hsourcefile,
        IntPtr hdestinationfile, IntPtr lpdata)
    {
        Offset = ((double)totalbytestransferred) / totalfilesize;
        Message = speedComputer.ReportInterval(totalbytestransferred);
        return CopyProgressResult.PROGRESS_CONTINUE;
    }
    
}