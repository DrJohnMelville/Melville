using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Melville.INPC;

namespace Melville.FileSystem.CopyWithProgress;

public static class CopyProgressImpl
{
    public static Task CopyAsync(
        string sourceFileName, string destFileName, CancellationToken token, CopyProgressRoutine copyProgressHandler)
    {
        int pbCancel = 0;
        token.ThrowIfCancellationRequested();
        var ctr = token.Register(() => pbCancel = 1);
        return Task.Run(() =>
        {
            try
            {
                CopyFileEx(sourceFileName, destFileName, copyProgressHandler, IntPtr.Zero, ref pbCancel, 
                    CopyFileFlags.COPY_FILE_ALLOW_DECRYPTED_DESTINATION |
                    CopyFileFlags.COPY_FILE_REQUEST_COMPRESSED_TRAFFIC);
                token.ThrowIfCancellationRequested();
            }
            finally
            {
                ctr.Dispose();
            }
        }, token);
    }


    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName,
       CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref Int32 pbCancel,
       CopyFileFlags dwCopyFlags);
}