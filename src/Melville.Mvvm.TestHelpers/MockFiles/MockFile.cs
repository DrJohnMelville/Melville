#nullable disable warnings
using  System.Runtime.CompilerServices;
using Melville.MVVM.FileSystem;

namespace Melville.Mvvm.TestHelpers.MockFiles
{
  public class MockFile : MemoryFile
  {

    public MockFile(string path, MemoryDirectory directory = null) : base(path, directory ?? new MockDirectory("z:\\Fake\\Mock\\Dir"))
    {
    }

    protected override void CheckFileThrow([CallerMemberName] string memberName = "") =>
      (Directory as MockDirectory)?.ThrowOnNextFileOperation?.Invoke(memberName);
  }
}