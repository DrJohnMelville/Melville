using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Melville.FileSystem;

public class MemoryFile : MemoryFileSystemObject, IFile
{
  public MemoryFile(string path, IDirectory? directory = null) : base(path)
  {
    Directory = directory ?? new MemoryDirectory("z:\\Fake\\Mem\\Dir");
  }

  public MemoryFile(string name, string resourceName, string prefix, Assembly assembly, IDirectory? directory = null) :
    this(name, directory)
  {
    using (var str = assembly.GetManifestResourceStream(prefix + resourceName))
    {
      SetFileData(str?.ReadToArray()??Array.Empty<byte>());
    }
  }

  public MemoryFile(string path, Stream sourceStream, IDirectory? directory = null) : this(path, sourceStream.ReadToArray(), directory)
  {
  }

  public MemoryFile(string path, byte[] sourceData, IDirectory? directory = null) : this(path, directory)
  {
    SetFileData(sourceData);
  }
  protected virtual void CheckFileThrow([CallerMemberName] string memberName = "")
  {
  }
    
  private byte[]? backingStore;

  public override string ToString()
  {
    return (Path ?? "") + ": " + Text();
  }
  private String Text() =>
    backingStore == null ? "<Does not Exist>" :
      Encoding.UTF8.GetString(backingStore, 0, backingStore.Length);

  public void Replace(string find, string replace) =>
    backingStore = Encoding.UTF8.GetBytes(Text().Replace(find, replace));
  public override bool Exists() => backingStore != null;

  public Task<Stream> OpenRead()
  {
    CheckFileThrow();
    if (backingStore == null)
    {
      CreateBackingStore();
    }
    LastAccess = DateTime.Now;
    // Backing store is not null because it is created in the guard clause above.
    return Task.FromResult((Stream) new MemoryStream(backingStore!));
  }

  protected virtual void CreateBackingStore()
  {
    throw new FileNotFoundException("Cannot find named file.", Path);
  }

  public Task<Stream> CreateWrite(FileAttributes attributes)
  {
    CheckFileThrow();
    backingStore = new byte[0]; // File exists immediately after being created
    Attributes = attributes;
    LastWrite = DateTime.Now;
    var writableStream = new MemoryStream();
    return Task.FromResult(new ExtendedCloseStream(writableStream, () =>
    {
      writableStream.Close();
      SetFileData(writableStream.ToArray());
    }) as Stream);
  }

  public void SetFileData(byte[] data)
  {
    backingStore = data;
  }

  public override void Delete()
  {
    backingStore = null;
  }
  private byte finalProgress = 255;
  public byte FinalProgress
  {
    get { return finalProgress; }
    set { finalProgress = value; }
  }
  public Task WaitForFinal
  {
    get { return Task<int>.FromResult(1); }
  }
    
  public long Size => backingStore == null ? 0: backingStore.Length;
}