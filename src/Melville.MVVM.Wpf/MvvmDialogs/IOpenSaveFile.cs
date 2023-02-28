using  System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Melville.FileSystem;
using Melville.INPC;
using Ookii.Dialogs.Wpf;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Melville.MVVM.Wpf.MvvmDialogs;

public interface IOpenSaveFile
{
  IDirectory? GetDirectory(string? dir = null);
  string? GetDirectoryString(string? dir = null);


  IFile? GetSaveFile(IDirectory? defaultDirectory, string ext, string filter, string title, string? name = null);
  String? GetSaveFileName(string? defaultDir, string ext, string filter, string title, string? name = null);

  string? GetLoadFileName(string? defaultDir, string ext, string filter, string title);
  IFile? GetLoadFile(IDirectory? defaultDir, string ext, string filter, string title);

  IEnumerable<IFile> GetLoadFiles(IDirectory? defaultDir, string ext, string filter, string title,
    bool oneFileOnly = false);

  IEnumerable<string> GetLoadFileNames(string? defaultDir, string ext, string filter, string title,
    bool oneFileOnly = false);

  string ImageFileFilter { get; }
}

public sealed partial class OpenSaveFileAdapter : IOpenSaveFile
{
  [FromConstructor]private readonly Window parentWindow;
  
  #region Load Folder names

  public IDirectory? GetDirectory(string? dir)
  {
    var str = GetDirectoryString(dir);
    return (str != null) ? new FileSystemDirectory(str) : null;
  }

  public string? GetDirectoryString(string? dir)
  {
    var dlg = new VistaFolderBrowserDialog();
    if (dir != null) dlg.SelectedPath = dir; 
    return (dlg.ShowDialog() ?? false) ? dlg.SelectedPath : null;
  }

  #endregion

  #region Save Files

  private static FileSystemDirectory _fileSystemDirectory = new FileSystemDirectory("C:\\");

  public IFile? GetSaveFile(IDirectory? defaultDirectory, string ext, string filter, string title, string? name = null)
  {
    var fileName = GetSaveFileName(defaultDirectory?.Path, ext, filter, title, name);
    return fileName == null ? null : NameToFile(defaultDirectory, fileName);
  }

  private static IFile NameToFile(IDirectory? defaultDirectory, string fileName)
  {
    return (defaultDirectory ?? _fileSystemDirectory).FileFromRawPath(fileName);
  }

  public String? GetSaveFileName(string? defaultDir, string ext, string filter, string title, string? name = null)
  {
    var dlg = new SaveFileDialog
    {
      AddExtension = true,
      CheckFileExists = false,
      CheckPathExists = true,
      DereferenceLinks = true,
      DefaultExt = ext,
      Filter = filter,
      Title = title,
      ValidateNames = true,
      InitialDirectory = defaultDir,
      FileName = name
    };
    return dlg.ShowDialog() ?? false ? dlg.FileName : null;
  }

  #endregion

  #region Load files

  public IFile? GetLoadFile(IDirectory? defaultDir, string ext, string filter, string title) =>
    GetLoadFiles(defaultDir, ext, filter, title, true).FirstOrDefault();

  public string? GetLoadFileName(string? defaultDir, string ext, string filter, string title) => 
    GetLoadFileNames(defaultDir, ext, filter, title, true).FirstOrDefault();

  public IEnumerable<IFile> GetLoadFiles(IDirectory? defaultDir, string ext, string filter, string title,
    bool oneFileOnly = false) => 
    GetLoadFileNames(defaultDir?.Path, ext, filter, title, oneFileOnly)
      .Select(i => NameToFile(defaultDir, i));

  public IEnumerable<string> GetLoadFileNames(string? defaultDir, string ext, string filter, string title, bool
    oneFileOnly = false)
  {
    var dlg = new OpenFileDialog
    {
      AddExtension = true,
      CheckFileExists = true,
      CheckPathExists = true,
      DefaultExt = ext,
      Filter = filter,
      InitialDirectory = defaultDir,
      Title = title,
      ValidateNames = true,
      Multiselect = !oneFileOnly,
      ShowReadOnly = false
    };
    var result = dlg.ShowDialog();
    return result ?? false ? dlg.FileNames : new string[0];
  }

  #endregion

  public string ImageFileFilter => 
    "Any Image File|*.jpg;*.jpeg;*.bmp;*.png;*.gif;*.tif;*.wdp|Joint Photographic Experts Group Image (*.jpg)|*.jpg|Bitmap (*.bmp)|*.bmp|Portable Network Graphic (*.png)|*.png|Graphics Interchange Format (*.gif)|*.gif|Tagged Image File Format (*.tif)|*.tif|Windows Metafile Bitmap (*.wdp)|*.wdp";
}

public sealed partial class CommandLineFileOpen : IOpenSaveFile
{
    [FromConstructor] private readonly string? cmdLine;
    [FromConstructor] private readonly IDiskFileSystemConnector connector;

    public IDirectory? GetDirectory(string? dir = null) =>
        GetDirectoryString(dir) is {} dirString ?connector.DirectoryFromPath(dirString):null;

    public string? GetDirectoryString(string? dir = null) => cmdLine;

    public IFile? GetSaveFile(
        IDirectory? defaultDirectory, string ext, string filter, string title, string? name = null)
        => FileOrNullFromPath(cmdLine);

    private IFile? FileOrNullFromPath(string? path) =>
        path is null ? null : connector.FileFromPath(path);

    public string? GetSaveFileName(string? defaultDir, string ext, string filter, string title, string? name = null) =>
        cmdLine;

    public string? GetLoadFileName(string? defaultDir, string ext, string filter, string title) =>
        cmdLine;

    public IFile? GetLoadFile(IDirectory? defaultDir, string ext, string filter, string title) =>
        FileOrNullFromPath(cmdLine);

    public IEnumerable<IFile> GetLoadFiles(IDirectory? defaultDir, string ext, string filter, string title, bool oneFileOnly = false) =>
        cmdLine is null? Array.Empty<IFile>(): new[] { connector.FileFromPath(cmdLine) };

    public IEnumerable<string> GetLoadFileNames(string? defaultDir, string ext, string filter, string title,
        bool oneFileOnly = false) =>
        cmdLine is null ? Array.Empty<string>(): new [] { cmdLine };

    public string ImageFileFilter => "";
}

public sealed partial class CompositeFileOpenSaveFile : IOpenSaveFile
{
    [FromConstructor] private IList<IOpenSaveFile> options;
    private int item = 0;

    [DelegateTo]
    private IOpenSaveFile Current => options[Math.Min(options.Count - 1, item++)];
}

