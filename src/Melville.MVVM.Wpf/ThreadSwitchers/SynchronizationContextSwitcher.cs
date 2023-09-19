using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Melville.INPC;

namespace Melville.MVVM.Wpf.ThreadSwitchers;

/// <summary>
/// Awaiting this struct will always resume on the given synchronization context, or
/// an arbitrary thread pool thread if the context is null
/// </summary>
public readonly partial struct SynchronizationContextSwitcher : INotifyCompletion
{
    public SynchronizationContextSwitcher GetAwaiter() => this;
    private static readonly SendOrPostCallback PostCallback = state => ((Action)state!)();
    [FromConstructor] private readonly SynchronizationContext? context;
    public bool IsCompleted => context == SynchronizationContext.Current;

    public void OnCompleted(Action continuation)
    {
        if (context is null)
            Task.Run(continuation);
        else
            context.Post(PostCallback, continuation);
    }

    public void GetResult()
    {
    }
}