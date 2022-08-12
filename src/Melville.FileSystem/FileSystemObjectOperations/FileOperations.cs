using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Melville.FileSystem.CopyWithProgress;

// for backwards compatability namespace does not match file system directory but the code is still better this way
namespace Melville.FileSystem;


public static partial class FileOperations
{
  //This code has an important performance optimization.  If both the source and the destination
  //are filesystem files then allow the file system to do what it does best.

  public static Task MoveFrom(this IFile destination, IFile source, CancellationToken token, FileAttributes attributes)
  {
    return CanUseFileSystemOptimization(destination, source) ? 
      SimpleFileOperations.MoveUsingFileSystem(attributes, destination.Path, source.Path) : 
      SimpleFileOperations.MoveUsingStreams(destination, source, token, attributes);
  }
  
  public static Task CopyFrom(this IFile destination, IFile source, CancellationToken token, FileAttributes attributes) => 
    CopyFrom(destination, source, token, attributes, NoProgressResult.Handler);

  public static Task CopyFrom(this IFile destination, IFile source, CancellationToken token, FileAttributes attributes,
    IProgress<double> progress) =>
    CopyFrom(destination, source, token, attributes, new CopyProgeressWrapper(progress).Handler);
  
  public static Task CopyFrom(this IFile destination, IFile source, CancellationToken token, FileAttributes attributes,
    CopyProgressRoutine progress) =>
   CanUseFileSystemOptimization(destination, source) ?
      CopyAndSetAttributes(destination, source, token, progress, attributes) :
      SimpleFileOperations.CopyUsingStreams(destination, source, token, attributes);

  private static bool CanUseFileSystemOptimization(IFile destination, IFile source) => 
    destination.ValidFileSystemPath() && source.ValidFileSystemPath();

  private static async Task CopyAndSetAttributes(
    IFile destination, IFile source, CancellationToken token, CopyProgressRoutine progress, 
    FileAttributes fileAttributes)
  {
    await CopyProgressImpl.CopyAsync(source.Path, destination.Path, token, progress);
    File.SetAttributes(destination.Path, fileAttributes);
  }
}