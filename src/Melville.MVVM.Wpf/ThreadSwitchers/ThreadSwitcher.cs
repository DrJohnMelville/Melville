using System.Threading;
using System.Windows.Threading;

namespace Melville.MVVM.Wpf.ThreadSwitchers;

public static class ThreadSwitcher
{
    public static DispatcherThreadSwitcher ResumeOnDispatcher(
        Dispatcher dispatcher, DispatcherPriority priority = DispatcherPriority.Background) =>
        new DispatcherThreadSwitcher(dispatcher, priority);

    public static SynchronizationContextSwitcher ResumeOnThreadPool() =>
        new SynchronizationContextSwitcher(null);

    public static SynchronizationContextSwitcher ResumeOnSynchronizationContext(
        SynchronizationContext? current) => new(current);

    /// <summary>
    /// Allows our clients to jump onto a dispatcher's thread by awaiting the dispatcher.
    /// </summary>
    public static DispatcherThreadSwitcher GetAwaiter(this Dispatcher dispatcher) =>
        ResumeOnDispatcher(dispatcher);

    /// <summary>
    /// Allows our clients to jump onto a Synchronization contnext's thread by awaiting the
    /// context.
    /// </summary>
    public static SynchronizationContextSwitcher GetAwaiter(this SynchronizationContext? sc) =>
        ResumeOnSynchronizationContext(sc);

}