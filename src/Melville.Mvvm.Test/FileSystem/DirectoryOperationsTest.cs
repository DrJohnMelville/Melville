#nullable disable warnings
using System.Linq;
using Melville.FileSystem;
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
    
    [Theory]
    [InlineData("a.txt", "b.txt", false)]
    [InlineData("a.txt", "a.txt", true)]
    [InlineData("*.txt", "a.txt", true)]
    [InlineData("*.txt", "akhfdkhgae.txt", true)]
    [InlineData("a*.txt", "akhfdkhgae.txt", true)]
    [InlineData("k*.txt", "akhfdkhgae.txt", false)]
    public void AllFilesTest(string glob, string name, bool exists)
    {
      dir.File(name).Create("Content");
      Assert.Equal(exists, dir.AllFiles(glob).Any());
    }


  }
}
