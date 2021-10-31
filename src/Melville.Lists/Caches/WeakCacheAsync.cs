using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Melville.Lists.Caches;

public class WeakCacheAsync<TKey, TResult> where TResult : class
{
  private readonly AsyncCache<TKey, WeakReference<TResult>> cache;
  private readonly Func<TKey, Task<TResult>> creator;

  public WeakCacheAsync(int size, Func<TKey, Task<TResult>> creator)
  {
    this.creator = creator;
    cache = new AsyncCache<TKey, WeakReference<TResult>>(size, MakeElement);
  }

  [MaybeNull]
  private TResult lastResult = default!;
  private async Task<WeakReference<TResult>> MakeElement(TKey key)
  {
    lastResult = await creator(key); // store in a field to make sure it sticks arround until we unpack the weakref
    return new WeakReference<TResult>(lastResult);
  }

  public async Task<TResult> Get(TKey key)
  {
    var reference = cache.Get(key);
    if (!(await reference).TryGetTarget(out var ret))
    {
      cache.Forget(key);
      ret = await Get(key); // will not loop because Forget means the TryGetTarget will succeed next time
    }
    lastResult = null!;
    return ret;
  }
}