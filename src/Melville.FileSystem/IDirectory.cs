using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem;

public static class DirectoryOperations
{
  public static void EnsureExists(this IDirectory dir, FileAttributes attributes = FileAttributes.Directory)
  {
    if (!dir.Exists())
    {
      dir.Create(attributes);
    }
  }

  public static IFile GetRandomFile(this IDirectory dir, string ext) => InnerGetRandomFile(dir, ext, out _);

  public static string GetRandomFileName(this IDirectory dir, string ext)
  {
    string name;
    InnerGetRandomFile(dir, ext, out name);
    return name;
  }

  private static readonly Random Random = new Random();
  private const string CharString = "abcdefghijklmnopqrstuvwxyz0123456789";
  private static IFile InnerGetRandomFile(IDirectory dir, string ext, out string name)
  {
    IFile file;
    do
    {
      name = RandomRileName(ext);
      file = dir.File(name);
    } while (file.Exists());
    return file;
  }

  public static string RandomRileName(string ext)
  {
    return $"{Guid.NewGuid()}.{ext}";
  }
    
  public static async Task DuplicateFrom(this IDirectory deestination, IDirectory source)
  {

    //I unrolled this recursive copy operation so it all stays inside one async block.
    Stack<(IDirectory dest, IDirectory source)> directories = new Stack<(IDirectory dest, IDirectory source)>();
    directories.Push((deestination, source));
    while (directories.Any())
    {
      var current = directories.Pop();
      foreach (var srcDir in current.source.AllSubDirectories())
      {
        var destDir = current.dest.SubDirectory(srcDir.Name);
        destDir.Create();
        directories.Push((destDir, srcDir));
      }

      foreach (var srcFile in current.source.AllFiles())
      {
        var destFile = current.dest.File(srcFile.Name);
        await destFile.CopyFrom(srcFile, CancellationToken.None, srcFile.Attributes);
      }
    }
  }
}