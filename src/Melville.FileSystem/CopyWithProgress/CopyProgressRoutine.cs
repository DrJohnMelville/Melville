using System;
using Melville.INPC;

namespace Melville.FileSystem.CopyWithProgress;

public delegate CopyProgressResult CopyProgressRoutine(
    long totalFileSize,
    long totalBytesTransferred,
    long streamSize,
    long streamBytesTransferred,
    uint dwStreamNumber,
    CopyProgressCallbackReason dwCallbackReason,
    IntPtr hSourceFile,
    IntPtr hDestinationFile,
    IntPtr lpData);
    
public partial class CopyProgressWrapper
{
    [FromConstructor] private readonly IProgress<double> progress;
    public CopyProgressResult Handler(
        long total, long transferred, long streamSize, long streamByteTrans, uint dwStreamNumber, 
        CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
    {
        progress.Report(((double)transferred)/total);
        return CopyProgressResult.PROGRESS_CONTINUE;
    }
}
public static class NoProgressResult
{
    public static CopyProgressResult Handler(
        long total, long transferred, long streamSize, long streamByteTrans, uint dwStreamNumber, 
        CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData) => 
        CopyProgressResult.PROGRESS_CONTINUE;
}
