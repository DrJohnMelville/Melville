using System;
using System.Collections.Generic;
using System.Windows;
using Melville.FileSystem;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;
using Melville.Log.Viewer.HomeScreens;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RootWindows;
using Melville.WpfAppFramework.StartupBases;
using Microsoft.Extensions.Configuration;

namespace Melville.Log.Viewer.ApplicationRoot;

public class Startup : StartupBase
{
    [STAThread]
    public static int Main(string[] commandLineArgs)
    {
        ApplicationRootImplementation.Run(new Startup());
        return 0;
    }

    protected override void RegisterWithIocContainer(IBindableIocService service)
    {
        SetupConfiguration(service);
        SetupMainWindowContent(service);
        SetupNugetFolderMonitoring(service);
    }

    private void SetupNugetFolderMonitoring(IBindableIocService service)
    {
        service.Bind<Func<Environment.SpecialFolder, string, IDirectory>>()
            .ToConstant(FolderFactory);
    }

    private IDirectory FolderFactory(Environment.SpecialFolder parent, string child) => 
        new FileSystemDirectory(Environment.GetFolderPath(parent) + child);

    private void SetupMainWindowContent(IBindableIocService service)
    {
        service.Bind<Application>().To<App>()
            .FixResult(i => ((App) i).InitializeComponent()).AsSingleton();
        service.RegisterHomeViewModel<HomeScreenViewModel>();
        service.Bind<RootNavigationWindow>().And<Window>().And<IRootNavigationWindow>()
            .ToSelf().FixResult(SetIcon).AsSingleton();
        service.Bind<IOpenSaveFile>().To<OpenSaveFileAdapter>();

    }

    private void SetIcon(RootNavigationWindow arg) => 
        arg.SetWindowIconFromResource("Melville.Log.Viewer", "ApplicationRoot/app.ico");

    private static void SetupConfiguration(IBindableIocService service)
    {
        service.AddConfigurationSources(i => i.AddUserSecrets<Startup>());
    }
}