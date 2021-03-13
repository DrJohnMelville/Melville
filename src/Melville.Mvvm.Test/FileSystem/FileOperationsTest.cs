#nullable disable warnings
using System;
using  System.IO;
using System.Threading;
using System.Threading.Tasks;
using Melville.Mvvm.TestHelpers.MockFiles;
using Melville.MVVM.FileSystem;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem
{
  public sealed class FileOperationsTest
  {
    [Fact]
    public void ExtensionTest()
    {
      InnerExtensionTest("jpg", "c:\\foo\\bar\\aaa.jpg");
      InnerExtensionTest("jpg", "c:\\foo\\bar\\aaa.bbb.jpg");
      InnerExtensionTest("j", "c:\\foo\\bar\\aaa.bbb.j");
      InnerExtensionTest("", ":\\foo\\bar\\aaabbbj");
      InnerExtensionTest("", ":\\foo\\bar\\aaabbbj.");
    }
    private static void InnerExtensionTest(string answer, string prompt)
    {
      var mock = new Mock<IFile>();
      mock.SetupGet(i => i.Path).Returns(prompt);
      Assert.Equal(answer, mock.Object.Extension());
    }

    [Fact]
    public async Task MoveFromFromTest()
    {
      var fs = new MockDirectory("x:\\Foo\\Bar");
      var srcFile = fs.File("foo.jpg");
      srcFile.Create("this is a test");
      var destFile = fs.File("dest.jpg");
      await (destFile.MoveFrom(srcFile, CancellationToken.None, FileAttributes.System));

      Assert.True(destFile.Exists());
      Assert.False(srcFile.Exists());
      Assert.Equal("this is a test", FileSystemHelperObjects.Content(destFile));
      Assert.Equal(FileAttributes.System, ((MockFile)destFile).Attributes);
    }
    [Fact]
    public async Task CopyFromFromTest()
    {
      var fs = new MockDirectory("x:\\Foo\\Bar");
      var srcFile = fs.File("foo.jpg");
      srcFile.Create("this is a test");
      var destFile = fs.File("dest.jpg");
      await (destFile.CopyFrom(srcFile, CancellationToken.None, FileAttributes.Temporary));

      Assert.True(destFile.Exists());
      Assert.True(srcFile.Exists());
      Assert.Equal("this is a test", FileSystemHelperObjects.Content(destFile));
      Assert.Equal(FileAttributes.Temporary, ((MockFile)destFile).Attributes);
    }

    // recognize paths on the same volume
    //From Wikipedias Path_(computing) 2/16/12
    //    Microsoft Windows uses the following types of paths:
    //       local file system (LFS), such as C:\File,
    //       uniform naming convention (UNC), such as \\Server\Volume\File,
    //       Long UNC or UNCW, such as \\?\C:\File or \\?\UNC\Server\Volume\File.

    [Fact]
    public void LocalPathVolumeComparison()
    {
      DoPathCompare(true, @"C:\foo.jpg", @"C:\bar.jpg", @"\\?\");
      DoPathCompare(true, @"C:\baz\foo.jpg", @"C:\bar.jpg", @"\\?\");
      DoPathCompare(true, @"C:\baz\foo.jpg", @"C:\bar", @"\\?\");
      DoPathCompare(false, @"D:\foo.jpg", @"C:\foo.jpg", @"\\?\");
      DoPathCompare(false, @"D:\foo.jpg", @"foo.jpg", @"\\?\");
      DoPathCompare(false, @"foo.jpg", @"C:\foo.jpg", @"\\?\");
    }
    [Fact]
    public void UNCPathVolumeComparison()
    {
      DoPathCompare(true, @"\\server\volume\dir\file.jpg", @"\\server\volume\dir\file.jpg", @"\\UNC\");
      DoPathCompare(true, @"\\server\volume\dir\file.jpg", @"\\server\volume\dir\fi1le.jpg", @"\\UNC\");
      DoPathCompare(true, @"\\server\volume\dir\file.jpg", @"\\server\volume\d1ir\file.jpg", @"\\UNC\");
      DoPathCompare(false, @"\\server\volume\dir\file.jpg", @"\\serve1r\volume\dir\file.jpg", @"\\UNC\");
      DoPathCompare(false, @"\\server\volume\dir\file.jpg", @"\\server\vo1lume\dir\file.jpg", @"\\UNC\");
    }
    private static void DoPathCompare(bool same, string pathA, string pathB, string prefix)
    {
      InnerDoPathCompare(same, pathA, pathB);
      InnerDoPathCompare(same, prefix + pathA, pathB);
      InnerDoPathCompare(same, pathA, prefix + pathB);
      InnerDoPathCompare(same, prefix + pathA, prefix + pathB);
    }
    private static void InnerDoPathCompare(bool same, string pathA, string pathB)
    {
      Assert.Equal(same, FileOperations.SameVolume(pathA, pathB));
      Assert.Equal(same, FileOperations.SameVolume(pathA.Replace('\\', '/'), pathB));
      Assert.Equal(same, FileOperations.SameVolume(pathA, pathB.Replace('\\', '/')));
      Assert.Equal(same, FileOperations.SameVolume(pathA.Replace('\\', '/'),
        pathB.Replace('\\', '/')));
    }

    [Theory]
    [InlineData("abcd.txt", "abcd.ext")]
    [InlineData("a.b.abcd.txt", "a.b.abcd.ext")]
    public void SisterFileText(string src, string result)
    {
      var mockDir = new MockDirectory("r:\\ss");
      var srcFile = mockDir.File(src);
      Assert.Equal("r:\\ss\\" + result, srcFile.SisterFile(".ext").Path);
    }

    [Fact]
    public async Task CopyEmptyDirectory()
    {
      var mockDirectory1 = new MockDirectory("c:\\Mock1");
      mockDirectory1.Create();
      var mockDirectory2 = new MockDirectory("c:\\Mock2");

      await mockDirectory2.CopyFrom(mockDirectory1);
      Assert.True(mockDirectory1.Exists());
      Assert.True(mockDirectory2.Exists());
    }

    [Fact]
    public async Task CopyWithSubDirectory()
    {
      var mockDirectory1 = new MockDirectoryTreeBuilder().Folder("F1").Object;
      mockDirectory1.Create();
      var mockDirectory2 = new MockDirectory("c:\\Mock2");

      await mockDirectory2.CopyFrom(mockDirectory1);
      Assert.True(mockDirectory1.SubDirectory("F1").Exists());
      Assert.True(mockDirectory2.SubDirectory("F1").Exists());
      
    }
    [Fact]
    public async Task CopyWithFile()
    {
      var mockDirectory1 = new MockDirectoryTreeBuilder().File("F1", "content").Object;
      mockDirectory1.Create();
      var mockDirectory2 = new MockDirectory("c:\\Mock2");

      await mockDirectory2.CopyFrom(mockDirectory1);
      Assert.True(mockDirectory1.File("F1").Exists());
      Assert.True(mockDirectory2.File("F1").Exists());
      Assert.Equal("content", mockDirectory2.File("F1").Content());
      
      
    }
    [Fact]
    public async Task MoveEmptyDirectory()
    {
      var mockDirectory1 = new MockDirectory("c:\\Mock1");
      mockDirectory1.Create();
      var mockDirectory2 = new MockDirectory("c:\\Mock2");

      await mockDirectory2.MoveFrom(mockDirectory1);
      Assert.False(mockDirectory1.Exists());
      Assert.True(mockDirectory2.Exists());
    }

    [Fact]
    public async Task MoveWithSubDirectory()
    {
      var mockDirectory1 = new MockDirectoryTreeBuilder().Folder("F1").Object;
      mockDirectory1.Create();
      var mockDirectory2 = new MockDirectory("c:\\Mock2");

      await mockDirectory2.MoveFrom(mockDirectory1);
      Assert.False(mockDirectory1.SubDirectory("F1").Exists());
      Assert.True(mockDirectory2.SubDirectory("F1").Exists());
      
    }
    [Fact]
    public async Task MoveWithFile()
    {
      var mockDirectory1 = new MockDirectoryTreeBuilder().File("F1", "content").Object;
      mockDirectory1.Create();
      var mockDirectory2 = new MockDirectory("c:\\Mock2");

      await mockDirectory2.MoveFrom(mockDirectory1);
      Assert.False(mockDirectory1.File("F1").Exists());
      Assert.True(mockDirectory2.File("F1").Exists());
      Assert.Equal("content", mockDirectory2.File("F1").Content());
    }
  }
}