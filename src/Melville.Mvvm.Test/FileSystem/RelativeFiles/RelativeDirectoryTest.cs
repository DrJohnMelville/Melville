#nullable disable warnings
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.FileSystem.PseudoTransactedFS;
using Melville.FileSystem.RelativeFiles;
using Melville.Mvvm.TestHelpers.MockFiles;
using Melville.Mvvm.TestHelpers.TestWrappers;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.RelativeFiles;

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
  public void AllFilesTest(string? glob, int files)
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
    [Fact]
    public async Task UntransactedRoot()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();

      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();
      Assert.Equal(baseFileSystem, txFs.UntransactedRoot);
      
    }
    [Fact]
    public async Task WriteTransactedFileSucceed()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();

      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();
      using (var txn = txFs.BeginTransaction())
      {
        txn.File("foo.jpg").Create("bar");
        await txn.Commit();
      }

      var file = baseFileSystem.File("foo.jpg");
      Assert.Single(baseFileSystem.AllFiles());
      Assert.True(file.Exists());
      Assert.Equal("bar", FileSystemHelperObjects.Content(file));
      
    }
    [Fact]
    public async Task WriteTransactedFileWithoutCommit()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();

      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();
      using (var txn = txFs.BeginTransaction())
      {
        txn.File("foo.jpg").Create("bar");
        Assert.Single(baseFileSystem.AllFiles());
      }

      Assert.Empty(baseFileSystem.AllFiles());
    }
   [Fact]
    public async Task CommitFlagWritenAndDeleted()
    {
      var dirMock = new MockDirectory("C:\\yyy");
      var ptm = new PseudoTransactedStore(dirMock);
      await ptm.RepairIncompleteTransactions();
      dirMock.File("xxx").Create("FooBar");
      var txn = ptm.BeginTransaction();
      txn.File("xxx").Create("xxxYYY");
      foreach (var file in dirMock.AllFiles("xxx*"))
      {
        ;
      }
      Assert.True(FileExists(dirMock, "xxx.*.txn"));
      await txn.Commit();
      txn.Dispose();
      Assert.True(dirMock.File("xxx").Exists());
      Assert.False(FileExists(dirMock, "xxx.*.txn"));
      Assert.False(FileExists(dirMock, "Committed.*.txn"));
    }

    private bool FileExists(IDirectory dir, string mask) => dir.AllFiles(mask).Any();
    [Fact]
    public async Task EmptyTransactionDoesNotWriteFlag()
    {
      var fileMock = new Mock<IFile>();
      var dirMock = new Mock<IDirectory>();
      dirMock.Setup(i => i.File("Committed.1.txn")).Returns(fileMock.Object);

      var ptm = new PseudoTransactedStore(dirMock.Object);
      await ptm.RepairIncompleteTransactions();
      var txn = ptm.BeginTransaction();
      await txn.Commit();
      txn.Dispose();
      dirMock.Verify(i=>i.File("Comitted.1.txn"), Times.Never());
      fileMock.Verify(i => i.CreateWrite(FileAttributes.Normal), Times.Never());
      fileMock.Verify(i => i.Delete(), Times.Never());
    }
    [Fact]
    public async Task WriteTransactedSubDirFileSucceed()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();

      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();
      using (var txn = txFs.BeginTransaction())
      {
        var dir = txn.SubDirectory("subDir");
        dir.Create();
        dir.File("foo.jpg").Create("bar");
        await txn.Commit();
      }

      var file = baseFileSystem.SubDirectory("subDir").File("foo.jpg");
      Assert.True(file.Exists());
      Assert.Equal("bar", FileSystemHelperObjects.Content(file));
      
    }
    [Fact]
    public async Task WriteTransactedSubDirFileWithoutCommit()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();

      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();
      baseFileSystem.SubDirectory("subDir").File("foo.jpg").Create("UneditedContent");
      using (var txn = txFs.BeginTransaction())
      {
        var dir = txn.SubDirectory("subDir");
        dir.Create();
        dir.File("foo.jpg").Create("bar");
      }

      var file = baseFileSystem.SubDirectory("subDir").File("foo.jpg");
      Assert.True(file.Exists());
      Assert.Equal("UneditedContent", FileSystemHelperObjects.Content(file));
    }
    [Fact]
    public async Task RollbackUnfinishedTransactionOnInitalize()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();
      baseFileSystem.File("foo.jpg.1.txn").Create("bar");

      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();

      Assert.False(baseFileSystem.File("foo.jpg.1.txn").Exists());
      Assert.False(baseFileSystem.File("foo.jpg").Exists());
      Assert.DoesNotContain(baseFileSystem.AllFiles(), i =>i.Exists());
    }
    [Fact]
    public async Task RollbackUnfinishedSubDirTransactionOnInitalize()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();
      var dir = baseFileSystem.SubDirectory("subDir");
      dir.Create();
      dir.File("foo.jpg.1.txn").Create("bar");
      
      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();

      Assert.False(baseFileSystem.SubDirectory("subDir").File("foo.jpg.1.txn").Exists());
      Assert.False(baseFileSystem.SubDirectory("subDir").File("foo.jpg").Exists());
      Assert.DoesNotContain(dir.AllFiles(), i =>i.Exists());
    }
    [Fact]
    public async Task CommitUnfinishedTransactionOnInitalize()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();
      baseFileSystem.File("foo.jpg.1.txn").Create("bar");
      baseFileSystem.File(TransactedDirectory.CommitFlagName+".1.txn").Create("   ");

      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();

      Assert.False(baseFileSystem.File("foo.jpg.1.txn").Exists());
      Assert.False(baseFileSystem.File(TransactedDirectory.CommitFlagName + ".1.txn").Exists());
      Assert.True(baseFileSystem.File("foo.jpg").Exists());
    }
    [Fact]
    public async Task CommitUnfinishedSubDirTransactionOnInitalize()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();
      var dir = baseFileSystem.SubDirectory("subDir");
      dir.Create();
      dir.File("foo.jpg.1.txn").Create("bar");
      baseFileSystem.File(TransactedDirectory.CommitFlagName + ".1.txn").Create("   ");

      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();

      Assert.False(baseFileSystem.SubDirectory("subDir").File("foo.jpg.1.txn").Exists());
      Assert.True(baseFileSystem.SubDirectory("subDir").File("foo.jpg").Exists());
      Assert.False(baseFileSystem.File(TransactedDirectory.CommitFlagName + ".1.txn").Exists());
    }
    [Fact]
    public async Task CommitMixedTransactionOnInitalize()
    {
      var baseFileSystem = new MockDirectory("C:\\Foo");
      baseFileSystem.Create();
      var dir = baseFileSystem.SubDirectory("subDir");
      dir.Create();
      dir.File("foo.jpg.1.txn").Create("bar");
      dir.File("baz.jph.2.txn").Create("bar");
      baseFileSystem.File(TransactedDirectory.CommitFlagName + ".1.txn").Create("   ");

      var txFs = new PseudoTransactedStore(baseFileSystem);
      await txFs.RepairIncompleteTransactions();

      Assert.False(baseFileSystem.SubDirectory("subDir").File("foo.jpg.1.txn").Exists());
      Assert.True(baseFileSystem.SubDirectory("subDir").File("foo.jpg").Exists());
      Assert.False(baseFileSystem.SubDirectory("subDir").File("baz.jpg.2.txn").Exists());
      Assert.False(baseFileSystem.SubDirectory("subDir").File("baz.jpg").Exists());
      Assert.False(baseFileSystem.File(TransactedDirectory.CommitFlagName + ".1.txn").Exists());
      Assert.False(baseFileSystem.File(TransactedDirectory.CommitFlagName + ".2.txn").Exists());
    }
  }
  