
namespace Melville.MVVM.FileSystem.PseudoTransactedFS
{
  public sealed class UntransactedStore: TransactionStoreBase, ITransactableStore
  {
    public UntransactedStore(IDirectory innerFileSystem) : base(innerFileSystem)
    {
    }

    public ITransactedDirectory BeginTransaction() => new PassthroughTransactedDirectory(innerFileSystem);
  }
}