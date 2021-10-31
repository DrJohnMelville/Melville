using System;
using System.Windows;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.RootWindows;
using Melville.WpfAppFramework.IocBridges;

namespace Melville.WpfAppFramework.StartupBases;

public static class ApplicationRootImplementation
{
    /// <summary>
    /// Run a WPF App from the model described by the startup object.
    /// You can Bind to Application to set the application object.
    /// Bind to IRootNavigationWindow to set the root window type
    /// Bind to IHomeViewModel to set the viewmodel of the default view.
    /// </summary>
    /// <param name="startUp"></param>
    public static void Run(StartupBase startUp)
    {
        var app = Application(startUp.Create());
        app.Run();
    }
        
    private static Application Application(IIocService root)
    {
        var application = root.Get<Application>();
        application.AttachDiRoot(new DiBridge(root));
        if (VisibleMainWindow(root) is Window mainWin)
        {
            application.MainWindow = mainWin;
        }
        return application;
    }

    private static IRootNavigationWindow VisibleMainWindow(IIocService root)
    {
        var ret = root.Get<IRootNavigationWindow>();
        ret.Show();
        TryToNavigateToHomeViewModel(root, ret);
        return ret;
    }

    private static void TryToNavigateToHomeViewModel(IIocService root, IRootNavigationWindow ret)
    {
        if (ret.DataContext is INavigationWindow window &&
            root.CanGet<IHomeViewModelFactory>() &&
            CreateHomeViewModel(root) is {} home)
        {
            window.NavigateTo(home);
        }
    }

    private static object? CreateHomeViewModel(IIocService root) => 
        root.Get<IHomeViewModelFactory>().Create(root);
}

public static class HomeViewRegistration
{
    public static IActivationOptions<IHomeViewModelFactory> RegisterHomeViewModel<T>(
        this IBindableIocService ioc) => ioc.RegisterHomeViewModel(i => i.Get<T>());

    public static IActivationOptions<IHomeViewModelFactory> RegisterHomeViewModel(
        this IBindableIocService ioc, Func<IIocService, object?> creator)
    {
        return ioc.Bind<IHomeViewModelFactory>()
            .ToConstant(new HomeViewModelFactoryNonDefaultImplementation(creator));
    }
}