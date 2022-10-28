using System;
using System.Threading.Tasks;
using Melville.INPC;

namespace Melville.FileSystem.PseudoTransactedFS;

public sealed partial class SwapableStore : ITransactableStore
{

  [FromConstructor][DelegateTo]private ITransactableStore innerStore;

  public async Task CopyAndTransferToNewStore(ITransactableStore newStore)
  {
    await newStore.UntransactedRoot.DuplicateFrom(innerStore.UntransactedRoot);
    innerStore = newStore;
  }
}