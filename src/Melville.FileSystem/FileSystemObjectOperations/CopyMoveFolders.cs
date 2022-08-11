using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem;

public static partial class FileOperations
{
    public static async Task CopyFrom(this IDirectory destination, IDirectory source, CancellationToken? token = null)
    {
        CancellationToken token1 = token ?? CancellationToken.None;
        destination.Create();
        foreach (var subDir in source.AllSubDirectories())
        {
            await CopyFrom(destination.SubDirectory(subDir.Name), subDir, token1);
        }

        foreach (var file in source.AllFiles())
        {
            await CopyFrom(destination.File(file.Name), file, token1, source.Attributes);
        }
    }

    public static Task MoveFrom(this IDirectory destination, IDirectory source, CancellationToken? token = null)
    {
        // this is a performance optimization -- let the OS do the Moveing for us
        return destination.ValidFileSystemPath() && source.ValidFileSystemPath()
            ? FileSystemMove(destination, source)
            : MoveUsingStreams(destination, source, token ?? CancellationToken.None);
    }

    private static Task FileSystemMove(IDirectory destination, IDirectory source) =>
        Task.Run(() => Directory.Move(source.Path, destination.Path));

    private static async Task MoveUsingStreams(IDirectory destination, IDirectory source, CancellationToken token)
    {
        destination.Create();
        foreach (var subDir in source.AllSubDirectories())
        {
            await MoveUsingStreams(destination.SubDirectory(subDir.Name), subDir, token);
        }

        foreach (var file in source.AllFiles())
        {
            await SimpleFileOperations.MoveUsingStreams(destination.File(file.Name), file, token, source.Attributes);
        }

        source.Delete();
    }
}