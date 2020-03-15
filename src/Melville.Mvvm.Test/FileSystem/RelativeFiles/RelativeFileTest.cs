#nullable disable warnings
using  System;
using System.Collections;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Melville.MVVM.FileSystem;
using Melville.MVVM.FileSystem.RelativeFiles;
using Melville.Mvvm.TestHelpers.TestWrappers;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.RelativeFiles
{
  public sealed class RelativeFileTest
  {
    [Fact]
    public void TestForwarding()
    {
      var tester = new WrapperTest<IFile>(target =>
      {
        var root = new Mock<IDirectory>();
        root.Setup(i => i.File("Test.txt")).Returns(target);
        return new RelativeFile(()=>root.Object, "Test.txt");
      });
      tester.RegisterTypeCreator(()=>new MemoryStream() as Stream);
      tester.AssertAllMethodsForward();
    }
  }
}