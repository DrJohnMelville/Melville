using System;
using System.Threading.Tasks;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.SystemInterface.Time;

namespace Melville.Wpf.Samples.ScopedMethodCalls;

public class DisposableDependency: IDisposable
{
    private ScopedMethodCallViewModel? model;
    public void Dispose()
    {
        if (model != null)
        {
            model.IsDisposed = true;
        }
    }

    public void RegisterDisposeOperation(ScopedMethodCallViewModel newModel)
    {
        newModel.IsInitialized = true;
        newModel.IsDisposed = false;
        model = newModel;
    }
}
public class ScopedMethodCallViewModel: NotifyBase
{
    private bool isInitialized;
    public bool IsInitialized 
    {
        get => isInitialized;
        set => AssignAndNotify(ref isInitialized, value);
    }
        
    private bool isDisposed;
    public bool IsDisposed 
    {
        get => isDisposed;
        set => AssignAndNotify(ref isDisposed, value);
    }

    public void Clear()
    {
        IsInitialized = false;
        IsDisposed = false;
    }

    public void RunVoid([FromServices] DisposableDependency d, [FromServices] IWallClock clock)
    {
        d.RegisterDisposeOperation(this);
    }
    public int RunInt([FromServices] DisposableDependency d, [FromServices] IWallClock clock)
    {
        d.RegisterDisposeOperation(this);
        return 1;
    }
    public bool RunBool([FromServices] DisposableDependency d, [FromServices] IWallClock clock)
    {
        d.RegisterDisposeOperation(this);
        return true;
    }
    public async ValueTask RunValueTask([FromServices] DisposableDependency d, [FromServices] IWallClock clock)
    {
        d.RegisterDisposeOperation(this);
        await clock.Wait(TimeSpan.FromSeconds(1));
    }
    public async ValueTask<int> RunValueTaskT([FromServices] DisposableDependency d, [FromServices] IWallClock clock)
    {
        d.RegisterDisposeOperation(this);
        await clock.Wait(TimeSpan.FromSeconds(1));
        return 1;
    }
    public async Task RunTask([FromServices] DisposableDependency d, [FromServices] IWallClock clock)
    {
        d.RegisterDisposeOperation(this);
        await clock.Wait(TimeSpan.FromSeconds(1));
    }
    public async Task<int> RunTaskT([FromServices] DisposableDependency d, [FromServices] IWallClock clock)
    {
        d.RegisterDisposeOperation(this);
        await clock.Wait(TimeSpan.FromSeconds(1));
        return 1;
    }
}