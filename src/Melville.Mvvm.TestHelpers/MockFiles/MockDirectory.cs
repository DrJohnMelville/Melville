#nullable disable warnings
using  System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Melville.FileSystem;

namespace Melville.Mvvm.TestHelpers.MockFiles
{
  public sealed class MockDirectory : MemoryDirectory
  {
    public Action<string> ThrowsOnNextDirectoryOperation { get; set; }
    public Action<string> ThrowOnNextFileOperation { get; set; }
    protected override void CheckForThrow([CallerMemberName] string memberName = "")
    {
      ThrowsOnNextDirectoryOperation?.Invoke(memberName);
    }
    public MockDirectory(string path) : base(path)
    {
    }
    protected override MemoryFile CreateFile(string name) => new MockFile(name, this);
    protected override MemoryDirectory MakeSubdirectoryObject(string name) => new MockDirectory(name);

    private bool isVolatile;
    public void SetVolitile(bool value) => isVolatile = value;
    public override bool IsVolitleDirectory() => isVolatile;

    #region Raw Paths
    private readonly Dictionary<string, IFile> rawPaths = new Dictionary<string, IFile>();
    public override IFile FileFromRawPath(string path)
    {
      IFile ret = null;
      rawPaths.TryGetValue(path, out ret);
      return ret;
    }

    public void AddRawFile(string path, IFile file)
    {
      rawPaths.Add(path, file);
    }
    #endregion
  }
}