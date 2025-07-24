using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
        FileAttributes attributes)
    {
        await using var src = await source.OpenRead();
        await using var dest = await destination.CreateWrite(attributes);
        if (src.Length is > 0 and var sourceLength)
            dest.SetLength(sourceLength);
        await src.CopyToAsync(dest, 10240, token);
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