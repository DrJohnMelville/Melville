using Melville.INPC;
using System;

namespace Melville.FileSystem;

/// <summary>
// Converts a file path into an IFile
/// </summary>
public interface IDiskFileSystemConnector
{
  /// <summary>
  /// Returns an IFile corresponding to a given file path
  /// </summary>
  /// <param name="path">the path of the file to retrieve</param>
  /// <returns>An IFile instance representing that file, which may not exist.</returns>
  IFile FileFromPath(string path);
  /// <summary>
  /// Returns an IDirectory corresponding to the given path
  /// </summary>
  /// <param name="path">The path to the directory.</param>
  /// <returns>IDirectory corresonding to the given path</returns>
  IDirectory DirectoryFromPath(string path);
}

public class DiskFileSystemConnector : IDiskFileSystemConnector
{
  public IFile FileFromPath(string path) => new FileSystemFile(path);
  public IDirectory DirectoryFromPath(string path) => new FileSystemDirectory(path);
}

public partial class EnvironmentExpandingDiskConnector : IDiskFileSystemConnector
{
    [FromConstructor] private readonly IDiskFileSystemConnector inner;

    public IFile FileFromPath(string path) => inner.FileFromPath(Expand(path));

    public IDirectory DirectoryFromPath(string path) => inner.DirectoryFromPath(Expand(path));

    private string Expand(string path) =>
        Environment.ExpandEnvironmentVariables(path);
}
