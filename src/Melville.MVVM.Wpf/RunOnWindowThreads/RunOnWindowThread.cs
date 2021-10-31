using System;
using System.Windows;
using System.Windows.Threading;

namespace Melville.MVVM.Wpf.RunOnWindowThreads;

public interface IRunOnWindowThread
{
    void Run(Action action, DispatcherPriority priority = DispatcherPriority.Send);
    T Run<T>(Func<T> action, DispatcherPriority priority = DispatcherPriority.Send);
}
public class RunOnWindowThread: IRunOnWindowThread
{
    private readonly Dispatcher dispatcher;

    public RunOnWindowThread(Window window)
    {
        dispatcher = window.Dispatcher;
    }
    public void Run(Action action, DispatcherPriority priority = DispatcherPriority.Send) => 
        dispatcher.Invoke(action, priority);

    public T Run<T>(Func<T> action, DispatcherPriority priority = DispatcherPriority.Send) => 
        dispatcher.Invoke(action, priority);
}