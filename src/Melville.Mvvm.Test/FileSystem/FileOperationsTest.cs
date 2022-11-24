#nullable disable warnings
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Melville.FileSystem;
using Melville.Mvvm.TestHelpers.MockFiles;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem;

#nullable enable
public sealed class FileOperationsTest
{
  [Theory]
  [InlineData("jpg", "c:\\foo\\bar\\aaa.jpg")]
  [InlineData("jpg", "c:\\foo\\bar\\aaa.bbb.jpg")]
  [InlineData("j", "c:\\foo\\bar\\aaa.bbb.j")]
  [InlineData("", ":\\foo\\bar\\aaabbbj")]
  [InlineData("", ":\\foo\\bar\\aaabbbj.")]
  private static void ExtensionTest(string answer, string prompt)
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
  public async Task  CopyFromFromTest()
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

  [Theory]
  [InlineData(true, @"C:\foo.jpg", @"C:\bar.jpg", @"\\?\")]
  [InlineData(true, @"C:\baz\foo.jpg", @"C:\bar.jpg", @"\\?\")]
  [InlineData(true, @"C:\baz\foo.jpg", @"C:\bar", @"\\?\")]
  [InlineData(false, @"D:\foo.jpg", @"C:\foo.jpg", @"\\?\")]
  [InlineData(false, @"D:\foo.jpg", @"foo.jpg", @"\\?\")]
  [InlineData(false, @"foo.jpg", @"C:\foo.jpg", @"\\?\")]
  [InlineData(true, @"\\server\volume\dir\file.jpg", @"\\server\volume\dir\file.jpg", @"\\UNC\")]
  [InlineData(true, @"\\server\volume\dir\file.jpg", @"\\server\volume\dir\fi1le.jpg", @"\\UNC\")]
  [InlineData(true, @"\\server\volume\dir\file.jpg", @"\\server\volume\d1ir\file.jpg", @"\\UNC\")]
  [InlineData(false, @"\\server\volume\dir\file.jpg", @"\\serve1r\volume\dir\file.jpg", @"\\UNC\")]
  [InlineData(false, @"\\server\volume\dir\file.jpg", @"\\server\vo1lume\dir\file.jpg", @"\\UNC\")]
  public void DoPathCompare(bool same, string pathA, string pathB, string prefix)
  {
    InnerDoPathCompare(same, pathA, pathB);
    InnerDoPathCompare(same, prefix + pathA, pathB);
    InnerDoPathCompare(same, pathA, prefix + pathB);
    InnerDoPathCompare(same, prefix + pathA, prefix + pathB);
  }
  private static void InnerDoPathCompare(bool same, string pathA, string pathB)
  {
    Assert.Equal(same, FileOperations.SameVolume(pathA, pathB));
    Assert.Equal(same, FileOperations.SameVolume(pathA.Replace('\\','/'), pathB));
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
    Assert.Equal("r:\\ss\\"+result, srcFile.SisterFile(".ext").Path);
  }
}