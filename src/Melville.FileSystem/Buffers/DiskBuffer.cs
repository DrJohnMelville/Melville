using System;
using System.IO;
using System.Threading.Tasks;

namespace Melville.FileSystem.Buffers;

public static class DiskBuffer
{
    public static ValueTask WriteViaSystemPath(IFile target, Action<string> generator) =>
        WriteViaSystemPath(target, s =>
        {
            generator(s);
            return new ValueTask();
        });
        
    public static ValueTask WriteViaSystemPath(IFile target, Func<string, ValueTask> generator) =>
        target.ValidFileSystemPath()?
            generator(target.Path):
            ShuttleViaTempFile(target, generator);

    private static async ValueTask ShuttleViaTempFile(IFile target, Func<string, ValueTask> generator)
    {
        var temporaryFileName = ComputeUniqueName(target.Extension());
        await generator(temporaryFileName);
        await using (var tempFileStream = new FileStream(temporaryFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        await using (var finalTargetStream = await target.CreateWrite())
        {
            await tempFileStream.CopyToAsync(finalTargetStream);
        }
        File.Delete(temporaryFileName);
    }

    private static string ComputeUniqueName(string extension) => 
        $"{Path.GetTempPath()}{new Guid()}.{extension}";
    
}