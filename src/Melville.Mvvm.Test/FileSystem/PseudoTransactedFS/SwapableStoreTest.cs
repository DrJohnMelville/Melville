#nullable disable warnings
using  System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Melville.FileSystem.FileSystem;
using Melville.FileSystem.FileSystem.PseudoTransactedFS;
using Melville.MVVM.Asyncs;
using Melville.Mvvm.TestHelpers.MockFiles;
using Melville.Mvvm.TestHelpers.TestWrappers;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.PseudoTransactedFS
{
  public sealed class SwapableStoreTest
  {
    [Fact]
    public void SwapableStorePreSwao()
    {
      new WrapperTest<ITransactableStore>(i => new SwapableStore(i)).AssertAllMethodsForward();
    }
    [Fact]
    public void SwapableStorePostSwao()
    {
      var initialStore = CreateBackedStore();
      var tester = new WrapperTest<ITransactableStore>(i =>
      {
        var swapable = new SwapableStore(initialStore.Object);
        AsyncPump.Run(() => swapable.CopyAndTransferToNewStore(i));
        return swapable;
      });
      tester.RegisterTypeCreator(()=>new MockDirectory("c:\\a") as IDirectory);
      tester.AssertAllMethodsForward();
      
      initialStore.VerifyGet(i=>i.UntransactedRoot, Times.Once);
      initialStore.VerifyNoOtherCalls();
    }

    private Mock<ITransactableStore> CreateBackedStore()
    {
      var initialStore = new Mock<ITransactableStore>();
      initialStore.Setup(i => i.UntransactedRoot).Returns(new MockDirectory("X:\\aaa"));
      return initialStore;
    }

    [Fact]
    public async Task SwapDuplicatesOriginal()
    {
      var source = CreateBackedStore();
      source.Object.UntransactedRoot.File("Hello.Txt").Create("eee");
      var ss = new SwapableStore(source.Object);
      var newStore = CreateBackedStore();
      await ss.CopyAndTransferToNewStore(newStore.Object);
      newStore.Object.UntransactedRoot.File("Hello.Txt").AssertContent("eee");
    }


  }
}