#nullable disable warnings
using System.IO;
using System.Linq;
using Melville.FileSystem;
using Melville.FileSystem.RelativeFiles;
using Melville.Mvvm.TestHelpers.MockFiles;
using Melville.Mvvm.TestHelpers.TestWrappers;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.RelativeFiles
{
  public sealed class RelativeDirectoryTest
  {
    [Fact]
    public void TestRelativeDirectory()
    {
      var tester = new WrapperTest<IDirectory>(target =>
      {
        var root = new Mock<IDirectory>();
        root.Setup(i => i.SubDirectory("Test.txt")).Returns(target);
        return new RelativeDirectory(()=>root.Object, "Test.txt");
      });
      tester.RegisterTypeCreator(()=>new MemoryStream() as Stream);
      tester.ExcludeMember(i=>i.File(""));
      tester.ExcludeMember(i=>i.SubDirectory(""));
      tester.ExcludeMember(i=>i.AllFiles());
      tester.ExcludeMember(i=>i.AllFiles("*.*"));
      tester.ExcludeMember(i=>i.AllSubDirectories());
      tester.AssertAllMethodsForward();
    }

    private readonly MockDirectory target = new MockDirectory("x:\\a");
    private readonly Mock<IDirectory> root = new Mock<IDirectory>();
    private readonly RelativeDirectory sut;

    public RelativeDirectoryTest()
    {
      root.Setup(i => i.SubDirectory("aaa")).Returns(target);
      sut = new RelativeDirectory(()=>root.Object, "aaa");
    }

    [Fact] 
    public void GetWrappedFile()
    {
      target.File("ab.txt").Create("Foo Bar");
      var file = sut.File("ab.txt"); 
      Assert.True(file is RelativeFile);
      file.AssertContent("Foo Bar");
    }
    
    [Fact] 
    public void GetWrappedDirectory()
    {
      var concreteDir = target.SubDirectory("c");
      concreteDir.Create();
      concreteDir.File("ab.txt").Create("Foo Bar");
      var dir = sut.SubDirectory("c");
      var file = dir.File("ab.txt"); 
      Assert.True(dir is RelativeDirectory);
      Assert.True(file is RelativeFile);
      file.AssertContent("Foo Bar");
    }

    [Theory]
    [InlineData("*.jpg", 2)]
    [InlineData("*.*", 4)]
    [InlineData(null, 4)]
    public void AllFilesTest(string glob, int files)
    {
      target.File("a.jpg").Create("a.jpg");
      target.File("B.jpg").Create("B.jpg");
      target.File("a.txt").Create("a.txt");
      target.File("B.txt").Create("B.txt");

      var ret = (glob == null?sut.AllFiles():sut.AllFiles(glob)).ToList();
      Assert.True(ret.All(i => i is RelativeFile));
      ret.ForEach(i=>i.AssertContent(i.Name));
      Assert.Equal(files, ret.Count());
    }

    [Fact]
    public void AllSubDirectoriesTest()
    {
      target.SubDirectory("aaa").Create();
      var dirs = sut.AllSubDirectories().ToList();
      Assert.Single(dirs);
      var dir = dirs.First();
      Assert.True(dir is RelativeDirectory);
      Assert.Equal("aaa", dir.Name);
    }

    [Fact]
    public void RelativeDirectoryRootTest()
    {
      target.File("ab.txt").Create("Foo Bar");
      var root = new RelativeDirectoryRoot(target);
      var file = root.File("ab.txt"); 
      Assert.True(file is RelativeFile);
      file.AssertContent("Foo Bar");
    }

  }
}