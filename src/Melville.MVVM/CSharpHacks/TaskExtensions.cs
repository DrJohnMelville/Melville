using  System;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.MVVM.CSharpHacks
{
  public static class TaskExtensions
  {
    public static Task<T> ThenDispose<T>(this Task<T> src, IDisposable target) =>
      src.ContinueWith(t =>
      {
        target.Dispose();
        return t.Result;
      });

    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken token, T sentinelValue)
    {
      var tcs = new TaskCompletionSource<bool>();
      using (token.Register(
                  s => (s as TaskCompletionSource<bool>)?.TrySetResult(true), tcs))
        if (task != await Task.WhenAny(task, tcs.Task))
          return sentinelValue;
      return await task;
    }

    // these empty methods eliminate the warning that we did not await a task;
    // it will be eliminated by the inliner (I hope.)
    public static void FireAndForget(this Task task) { }
    public static void FireAndForget<T>(this Task<T> task) {}
  }
}