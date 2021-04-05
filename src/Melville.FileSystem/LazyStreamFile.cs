using System;
using System.IO;

namespace Melville.FileSystem
{
  public sealed class LazyStreamFile : MemoryFile
  {
    private readonly Func<Stream> getSourceStream;

    public LazyStreamFile(string path, Func<Stream> getSourceStream, IDirectory? dir = null) : 
      base(path, dir)
    {
      this.getSourceStream = getSourceStream;
    }

    protected override void CreateBackingStore()
    {
      using var str = getSourceStream();
      SetFileData(str.ReadToArray());
    }

    public override bool Exists() => true;
  }
}