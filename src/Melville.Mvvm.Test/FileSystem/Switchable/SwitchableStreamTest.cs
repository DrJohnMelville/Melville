#nullable disable warnings
using  System;
using System.IO;
using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.FileSystem.Switchabe;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.Switchable;

public sealed class SwitchableStreamTest
{
  [Fact]
  public async Task SwitchOpenStream()
  {
    var ms = new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5 });
    var ms2 = new MemoryStream(new byte[] { 0, 10, 20, 30, 40, 50 });

    var data = new byte[10];
    var ss = new SwitchableStream(ms);
    ss.Read(data, 0, 3);
    await ss.SwitchSource(()=>Task.FromResult(ms2 as Stream));
    ss.Read(data, 3, 3);
    for (int i = 0; i < 6; i++)
    {
      Assert.Equal(i * (i > 2?10:1) , data[i]);
    }
  }
}

public abstract class SwitchableFileTest
{
  private readonly Mock<IFile> src;
  private readonly Mock<IFile> dest;
  private readonly SwitchableFileBase sut;

  protected SwitchableFileTest(Mock<IFile> src, Mock<IFile> dest, SwitchableFileBase sut)
  {
    this.src = src;
    this.dest = dest;
    this.sut = sut;
  }

  [Fact]
  public void PathPassthrough()
  {
    src.SetupGet(i => i.Path).Returns("Foo");
    Assert.Equal("Foo", sut.Path);
    src.VerifyGet(i => i.Path, Times.Exactly(1));
  }
  [Fact]
  public void ExistsPassthrough()
  {
    src.Setup(i => i.Exists()).Returns(true);
    Assert.True(sut.Exists());
    src.Verify(i => i.Exists(), Times.Exactly(1));
  }

  [Fact]
  public Task ThrowOnWrite()
  {
    return Assert.ThrowsAsync<NotSupportedException>(() => sut.CreateWrite(FileAttributes.Normal));
  }
  [Fact]
  public void ThrowOnDelete()
  {
    Assert.Throws<NotSupportedException>(() => sut.Delete());
  }

  [Fact]
  public async Task SimpleOpenRead()
  {
    var ms = new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5 });
    src.Setup(i => i.Exists()).Returns(true);
    src.Setup(s => s.OpenRead()).Returns(Task<Stream>.FromResult((Stream) ms));

    using (var s = await sut.OpenRead())
    {
      var buf = new byte[6];
      s.Read(buf, 0, 6);
      for (int i = 0; i < 6; i++)
      {
        Assert.Equal(i, buf[i]);
      }
    }
  }
    
  [Fact]
  public async Task DoubleOpenRead()
  {
    var ms = new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5 });
    src.Setup(s => s.OpenRead()).Returns(Task<Stream>.FromResult((Stream) ms));
    using (var s = await sut.OpenRead())
    using (var s2 = await sut.OpenRead())
    {
      var buf = new byte[6];
      s.Read(buf, 0, 2);
      s2.Read(buf, 2, 4);
      for (int i = 0; i < 6; i++)
      {
        Assert.Equal(i, buf[i]);
      }
    }
  }

  [Fact]
  public async Task SwitchSource()
  {
    var ms = new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5 });
    var ms2 = new MemoryStream(new byte[] { 10, 11, 12, 13, 14, 15 });
    var ret = new byte[6];
    var answer = new byte[] {0, 1, 2, 13, 14, 16};

    src.Setup(i => i.Exists()).Returns(true);
    src.Setup(i => i.OpenRead()).Returns(Task<Stream>.FromResult((Stream) ms));
    dest.Setup(i => i.Exists()).Returns(true);
    dest.Setup(i => i.OpenRead()).Returns(Task<Stream>.FromResult((Stream) ms2));

    var s = await sut.OpenRead();

    await s.ReadAsync(ret, 0, 3);
    await sut.Relocate(_ => { });
    await s.ReadAsync(ret, 3, 3);
    s.Dispose();
    Assert.True(sut.IsSwitched);
    Assert.Equal(answer, ret);      
  }
}
/*
public sealed class SwitchableCopyFileTest : SwitchableFileTest
{
  public SwitchableCopyFileTest() : this(new Mock<IFile>(), new Mock<IFile>()) { }

  private SwitchableCopyFileTest(Mock<IFile> src, Mock<IFile> dest) : 
    base(src, dest, new SwitchableCopyFile(src.Object, dest.Object)) { }
}

public sealed class SwitchableMoveFileTest : SwitchableFileTest
{
  public SwitchableMoveFileTest() : this(new Mock<IFile>(), new Mock<IFile>()) { }

  private SwitchableMoveFileTest(Mock<IFile> src, Mock<IFile> dest) : 
    base(src, dest, new SwitchableMoveFile(src.Object, dest.Object)) { }
}
*/