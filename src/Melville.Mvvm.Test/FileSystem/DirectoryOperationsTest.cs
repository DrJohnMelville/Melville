#nullable disable warnings
using Melville.MVVM.FileSystem;
using Melville.Mvvm.TestHelpers.MockFiles;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem
{
  public sealed class DirectoryOperationsTest
  {
    private readonly MockDirectory dir = new MockDirectory("m:\\sgfkdnjh");
    [Fact]
    public void CreateRandomName()
    {
      Assert.Matches(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}\.xyz$", dir.GetRandomFile("xyz").Name);
    }

    [Fact]
    public void MakePhotoFile()
    {
      IDirectory root = new MockDirectory("x:\\rootDir");
      var f = root.NewPhotoFile("jpg");
      f.Create("sss");
      Assert.Equal("jpg", f.Extension());
      Assert.Single(root.SubDirectory("Photos").AllFiles());
    }


  }
}
