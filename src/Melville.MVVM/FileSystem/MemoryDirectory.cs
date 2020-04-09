using  System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Melville.MVVM.CSharpHacks;

namespace Melville.MVVM.FileSystem
{
  public class MemoryDirectory : MemoryFileSystemObject, IDirectory
  {
    private readonly Dictionary<string, MemoryDirectory> subDirectories = new Dictionary<string, MemoryDirectory>();
    private readonly Dictionary<string, IFile> files = new Dictionary<string, IFile>();
    private bool exists;

    public MemoryDirectory(string path) : base(path) { }


    protected virtual void CheckForThrow([CallerMemberName] string memberName = "")
    {
    }

    public override bool Exists()
    {
      CheckForThrow();
      return exists;
    }

    public IEnumerable<IDirectory> AllSubDirectories()
    {
      CheckForThrow();
      return subDirectories.Values;
    }

    public void Create(FileAttributes attributes = FileAttributes.Directory)
    {
      CheckForThrow();
      Attributes = attributes | FileAttributes.Directory;
      exists = true;
    }

    public IDirectory SubDirectory(string name)
    {
      CheckForThrow();
      if (!subDirectories.TryGetValue(name, out var ret))
      {
        ret = MakeSubdirectoryObject(SubPathName(name));
        ret.Directory = this;
        subDirectories[name] = ret;
      }
      return ret;
    }

    protected virtual MemoryDirectory MakeSubdirectoryObject(string name) => new MemoryDirectory(name);

    private string SubPathName(string name) => System.IO.Path.Combine(Path, name);

    public IFile File(string name)
    {
      CheckForThrow();
      if (!files.TryGetValue(name, out var ret))
      {
        ret = CreateFile(SubPathName(name));
        files[name] = ret;
      }
      return ret;
    }

    protected virtual MemoryFile CreateFile(string name) => new MemoryFile(name, this);

    public IFile FileRecord(string name)
    {
      var regEx = MatchGlob(name);
      return files.Keys.Where(i => regEx.Match(i).Success).Select(i => files[i]).FirstOrDefault();
    }

    public IEnumerable<IFile> AllFiles()
    {
      CheckForThrow();
      return files.Values.Where(i => i.Exists());
    }

    public static Regex MatchGlob(string glob)
    {
      return new Regex($"^{RegexExtensions.GlobToRexex(glob)}$", RegexOptions.IgnoreCase);
    }

    public IEnumerable<IFile> AllFiles(string glob)
    {
      var regex = MatchGlob(glob);
      return AllFiles().Where(i => regex.IsMatch(i.Path));
    }

    public async Task AddFileExplicit(string path)
    {
      var mockFile = new MemoryFile(path, this);
        using (var str = await mockFile.CreateWrite(FileAttributes.Normal))
        {
          var buffer = Encoding.UTF8.GetBytes("Fake FileContent");
          await str.WriteAsync(buffer, 0, buffer.Length);
        }
      AddFileExplicit(path, mockFile);
    }

    public void AddFileExplicit(string path, IFile file)
    {
      files[path] = file;
    }

    public virtual IFile FileFromRawPath(string path)
    {
      return new FileSystemFile(path);
    }

    public virtual bool IsVolitleDirectory() => true;

    private IDisposable? writeToken;
    public IDisposable WriteToken()
    {
      return writeToken ??= new WriteTokenImpl(this);
    }

    private class WriteTokenImpl : IDisposable
    {
      private MemoryDirectory dir;

      public WriteTokenImpl(MemoryDirectory dir)
      {
        this.dir = dir;
      }
      
      public void Dispose()
      {
        dir.writeToken = null;
      }
    }

    public override void Delete()
    { 
      subDirectories.Clear();
      files.Clear();
      exists = false;
    }
  }
}