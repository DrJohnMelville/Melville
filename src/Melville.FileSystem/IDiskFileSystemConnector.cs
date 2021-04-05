namespace Melville.FileSystem
{
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
    IDirectory DirectoryFromPath(string path);
  }

  public class DiskFileSystemConnector : IDiskFileSystemConnector
  {
    public IFile FileFromPath(string path) => new FileSystemFile(path);
    public IDirectory DirectoryFromPath(string path) => new FileSystemDirectory(path);
  }

}