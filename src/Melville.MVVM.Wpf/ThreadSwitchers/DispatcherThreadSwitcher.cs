using System;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Melville.INPC;

namespace Melville.MVVM.Wpf.ThreadSwitchers;

/// <summary>
/// Awaiting this struct will always resume on the given Dispatcher
/// </summary>
public readonly partial struct DispatcherThreadSwitcher : INotifyCompletion
{
    [FromConstructor] private readonly Dispatcher dispatcher;
    [FromConstructor] private readonly DispatcherPriority priority;

    public DispatcherThreadSwitcher GetAwaiter() => this;
    public bool IsCompleted => dispatcher.CheckAccess();

    public void GetResult()
    {
    }

    public void OnCompleted(Action continuation) =>
        dispatcher.Invoke(continuation, priority);
}