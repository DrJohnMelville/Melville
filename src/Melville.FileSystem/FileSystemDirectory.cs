using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Melville.FileSystem
{
  public abstract class FileSystemObject<T>: IFileSystemObject where T : FileSystemInfo
  {
    protected T info;

    protected FileSystemObject(T info)
    {
      this.info = info;
    }

    public string Path => info.FullName;
    public string Name => info.Name;
    public bool Exists() => info.Exists;
    public bool ValidFileSystemPath() => true;
    public void Delete()
    {
      if (!Exists()) return;
      try
      {
        info.Delete();
      }
      catch (IOException)
      {
        // sometimes we cannot delete a file because we still have it cached.  We will just fix it next time.
      }
    }

    public DateTime LastAccess
    {
      get => info.LastAccessTime;
      set
      {
        try
        {
          info.LastAccessTime = value;
        }
        catch (IOException)
        {
        }
      }
    }
    public DateTime LastWrite => info.LastWriteTime;
    public DateTime Created => info.CreationTime;
    public FileAttributes Attributes => info.Attributes;
    public abstract IDirectory? Directory { get; }
  }
  public sealed class FileSystemDirectory : FileSystemObject<DirectoryInfo>, IDirectory
  {
    public FileSystemDirectory(string path):this (new DirectoryInfo(path)){}
    public FileSystemDirectory(DirectoryInfo di):base(di){}
    
    public IEnumerable<IDirectory> AllSubDirectories() => 
      info.EnumerateDirectories().Select(i => new FileSystemDirectory(i));

    public void Create(FileAttributes attributes)
    {
      info.Create();
      info.Attributes = FileAttributes.Directory | attributes;
    }

    public override IDirectory? Directory => info.Parent == null?null: new FileSystemDirectory(info.Parent);

    public IFile FileFromRawPath(string path) => new FileSystemFile(path);
    public bool IsVolitleDirectory() => false;
    public IDirectory SubDirectory(string name) => new FileSystemDirectory(SubPath(name));
    private string SubPath(string name) => System.IO.Path.Combine(Path, name);
    public IFile File(string name) => FileFromRawPath(SubPath(name));
    public IEnumerable<IFile> AllFiles() =>
      Exists()
        ? (IEnumerable<IFile>) info.EnumerateFiles().Select(i => new FileSystemFile(i))
        :  new IFile[0];
        
    public IEnumerable<IFile> AllFiles(string glob) =>
      Exists()
        ? (IEnumerable<IFile>) info.EnumerateFiles(glob).Select(i => new FileSystemFile(i))
        : new IFile[0];

    public IDisposable? WriteToken()
    {
      try
      {
        return System.IO.File.Create(SubPath("mutex.txt"), 10, FileOptions.DeleteOnClose);
      }
      catch (Exception)
      {
        return null;
      }
    }
  }

  public sealed class FileSystemFile : FileSystemObject<FileInfo>, IFile
  {
    public FileSystemFile(FileInfo fi): base(fi){}
    public FileSystemFile(string path):this(new FileInfo(path)){}
    
    public Task<Stream> OpenRead() => Task.FromResult<Stream>(info.OpenRead());
    
    public Task<Stream> CreateWrite(FileAttributes attributes) => Task.FromResult(CreateFile(attributes));

    private Stream CreateFile(FileAttributes attributes)
    {
      Stream ret = info.Create();
      if (attributes != FileAttributes.Normal)
      {
        ret = new ExtendedCloseStream(ret, () =>
        {
          if (Exists())
          {
            File.SetAttributes(Path, attributes);
          }
        });
      }
      return ret;
    }
    
    public byte FinalProgress => 255;
    public Task WaitForFinal => Task.FromResult(1);
    public long Size => info.Length;
    public override IDirectory Directory => new FileSystemDirectory(info.Directory ??
         throw new InvalidOperationException("File does not have a directory."));
  }
}