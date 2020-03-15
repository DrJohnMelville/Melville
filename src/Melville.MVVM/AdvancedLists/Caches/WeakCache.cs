using  System;
using System.Diagnostics.CodeAnalysis;

namespace Melville.MVVM.AdvancedLists.Caches
{
  public sealed class WeakCache<TKey, TResult> where TResult : class
  {
    private readonly SimpleCache<TKey, WeakReference<TResult>> cache;
    private readonly Func<TKey, TResult> creator;

    public WeakCache(int size, Func<TKey, TResult> creator)
    {
      this.creator = creator;
      cache = new SimpleCache<TKey, WeakReference<TResult>>(size, MakeElement);
    }

    [MaybeNull]
    private TResult lastResult = default!;
    private WeakReference<TResult> MakeElement(TKey key)
    {
      lastResult = creator(key); // store in a field to make sure it sticks arround until we unpack the weakref
      return new WeakReference<TResult>(lastResult);
    }

    public TResult Get(TKey key)
    {
      var reference = cache.Get(key);
      if (!reference.TryGetTarget(out var ret))
      {
        cache.Forget(key);
        ret = Get(key); // will not loop because Forget means the TryGetTarget will succeed next time
      }
      lastResult = null!;
      return ret;
    }
  }
}