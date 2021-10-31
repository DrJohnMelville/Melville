using System;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.Hacks;

/// <summary>
/// Run an action on a new background threat which uses the STA threadding model.
/// </summary>
public interface IStaWorker
{
  /// <summary>
  /// Run an async function on a STA background thread.
  /// </summary>
  /// <typeparam name="T">Type of the return value</typeparam>
  /// <param name="action">async function to run</param>
  /// <returns>The return value of the underlying method.</returns>
  Task<T> Run<T>(Func<Task<T>> action);
}

public static class StaWorkerOperations
{
  /// <summary>
  /// Run an async action on a STA background thread.
  /// </summary>
  /// <param name="action">async action to run</param>
  /// <returns>A task representing the underlying method</returns>
  public static Task Run(this IStaWorker worker, Func<Task> action) =>
    worker.Run(async () =>
    {
      await action();
      return 1;
    });

  /// <summary>
  /// Run a synchronous function on a STA background thread.
  /// </summary>
  /// <typeparam name="T">Type of the return value</typeparam>
  /// <param name="action">Function to run</param>
  /// <returns>The return value of the underlying method.</returns>
  public static Task Run<T>(this IStaWorker worker, Func<T> action) =>
    worker.Run(() => Task.FromResult(action()));

  /// <summary>
  /// Run a synchronous action on a STA background thread.
  /// </summary>
  /// <param name="action">Synchronous action to run</param>
  /// <returns>A task representing completion of the synchronous action on the other thread.</returns>
  public static Task Run(this IStaWorker worker, Action action) =>
    worker.Run(() => {
      action();
      return Task.FromResult(1);
    });
}

public class StaWorker : IStaWorker
{
  public Task<T> Run<T>(Func<Task<T>> action)
  {
    var tcs = new TaskCompletionSource<T>();
    var thread = new Thread(async () =>
    {
      tcs.SetResult(await action());
    });
      
    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    return tcs.Task;
  }
}