using System;
using System.Threading.Tasks;

namespace Melville.FileSystem.FileSystem.PseudoTransactedFS
{
  public sealed class SwapableStore : ITransactableStore
  {
    public SwapableStore(ITransactableStore innerStore)
    {
      this.innerStore = innerStore;
    }

    #region Delegating functions

    private ITransactableStore innerStore;
    public IDirectory UntransactedRoot => innerStore.UntransactedRoot;

    public ITransactedDirectory BeginTransaction()
    {
      return innerStore.BeginTransaction();
    }

    public bool IsLocalStore => innerStore.IsLocalStore;
    public IDownloadProgressStore? ProgressStore => innerStore.ProgressStore;

    public ValueTask DisposeAsync() => innerStore.DisposeAsync();

    public Task<bool> RenewLease()
    {
      return innerStore.RenewLease();
    }
    #endregion

    public async Task CopyAndTransferToNewStore(ITransactableStore newStore)
    {
      await newStore.UntransactedRoot.DuplicateFrom(innerStore.UntransactedRoot);
      innerStore = newStore;
    }

    public IDisposable? WriteToken() => innerStore.WriteToken();
  }
}