using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Melville.FileSystem.CopyWithProgress;

namespace Melville.FileSystem;

public static class SimpleFileOperations
{
    public static Task FileSystemCopy(string destinationPath, string sourcePath, FileAttributes attributes)
    {
        return Task.Run(() =>
        {
            File.Delete(destinationPath);
            File.Copy(sourcePath, destinationPath);
            File.SetAttributes(destinationPath, attributes);
        });
    }

    public static async Task CopyUsingStreams(IFile destination, IFile source, CancellationToken token,
        FileAttributes attributes, CopyProgressRoutine progress)
    {
        await using var src = await source.OpenRead();
        await using var dest = await destination.CreateWrite(attributes);
        if (src.Length > 0) dest.SetLength(src.Length);

        await CopyWithProgressAsync(src, dest, progress, token);
    }

    private static async Task CopyWithProgressAsync(Stream src, Stream dest, CopyProgressRoutine progress,
        CancellationToken token)
    {
        var readingBuffer = ArrayPool<byte>.Shared.Rent(81920);
        var writingBuffer= ArrayPool<byte>.Shared.Rent(81920);
        int bytesToWrite = 0;
        try
        {
            long totalBytesRead = 0;
            while (!token.IsCancellationRequested)
            {
                var writingTask = bytesToWrite > 0
                    ? dest.WriteAsync(writingBuffer, 0, bytesToWrite, token)
                    : Task.CompletedTask;
                var bytesRead = await src.ReadAsync(readingBuffer, 0, readingBuffer.Length, token);
                await writingTask;
                if (bytesRead == 0) break;
                totalBytesRead += bytesRead;
                bytesToWrite = bytesRead;
                (readingBuffer, writingBuffer) = (writingBuffer, readingBuffer);

                var progCall = progress(src.Length, totalBytesRead,
                    src.Length, totalBytesRead, 1, CopyProgressCallbackReason.CALLBACK_CHUNK_FINISHED,
                    IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                switch (progCall)
                {
                    case CopyProgressResult.PROGRESS_CONTINUE:
                    case CopyProgressResult.PROGRESS_QUIET:
                        break;
                    case CopyProgressResult.PROGRESS_STOP:
                    case CopyProgressResult.PROGRESS_CANCEL:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(readingBuffer);
            ArrayPool<byte>.Shared.Return(writingBuffer);
        }
    }

    public static async Task MoveUsingFileSystem(FileAttributes attributes, string destinationPath, string sourcePath)
    {
        if (FileOperations.SameVolume(destinationPath, sourcePath))
        {
            await Task.Run(() =>
            {
                // this is a guard clause for a bad bug I can't duplicate.  See issue # 79
                if (File.Exists(sourcePath))
                {
                    File.Delete(destinationPath);
                    File.Move(sourcePath, destinationPath);
                    if (attributes != FileAttributes.Normal)
                    {
                        File.SetAttributes(destinationPath, attributes);
                    }
                }
            });
            return;
        }
        // else
        await SimpleFileOperations.FileSystemCopy(destinationPath, sourcePath, attributes);
        File.Delete(sourcePath);
    }

    public static async Task MoveUsingStreams(IFile destination, IFile source, CancellationToken token,
        FileAttributes attributes)
    {
        // this is a guard clause for a bad bug I can't duplicate.  See issue # 79
        Debug.Assert(source.Exists());
        if (!source.Exists()) return;
        await FileOperations.CopyFrom(destination, source, token, attributes);
        source.Delete();
    }
}