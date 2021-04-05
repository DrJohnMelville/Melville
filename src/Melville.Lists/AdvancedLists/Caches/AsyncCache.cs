using System;
using System.Threading.Tasks;

namespace Melville.Lists.AdvancedLists.Caches
{
  public class AsyncCache<TKey, TResult>
  {
    private readonly SimpleCache<TKey, TaskHolder> cache;
    public AsyncCache(int size, Func<TKey, Task<TResult>> creator)
    {
      cache = new SimpleCache<TKey, TaskHolder>(size, k => new TaskHolder(creator(k)));
    }

    public Task<TResult> Get(TKey key) => cache.Get(key).Task;

    private class TaskHolder : IDisposable
    {
      public Task<TResult> Task;

      public TaskHolder(Task<TResult> task)
      {
        Task = task;
      }

      public void Dispose()
      {
        (Task.Result as IDisposable)?.Dispose();
      }
    }

    public void Forget(TKey key) => cache.Forget(key);
  }
}