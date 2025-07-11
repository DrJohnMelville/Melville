﻿using System;
using System.Windows;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RootWindows;
using Melville.WpfAppFramework.StartupBases;
using Microsoft.Extensions.Configuration;
using WebDashboard.NugetManager;

namespace WebDashboard.Startup;

public class Startup : StartupBase
{
    [STAThread]
    public static int Main(string[] args)
    {
        ApplicationRootImplementation.Run(new Startup(args));
        return 0;
    }

    public Startup(string[] commandLineParameters) : base(commandLineParameters)
    {
    }

    protected override void RegisterWithIocContainer(IBindableIocService service)
    {
        ConfigureNugetUpload(service);
        // window selectors
        service.Bind<IFileViewerFactory>().To<NugetManagerViewModelFactory>();
        service.Bind<IFileViewerFactory>().To<SecretManagerViewModelFactory>();
  
        // Root Window
        service.Bind<INavigationWindow>().To<NavigationWindow>().AsSingleton();
        service.Bind<RootNavigationWindow>().And<Window>().And<IRootNavigationWindow>()
            .ToSelf().FixResult(SetIcon).AsSingleton();
        service.RegisterHomeViewModel<FileLoadViewModel>();
            
        // System Services
        service.Bind<IStartupData>().To<StartupData>()
            .WithParameters(new object[] {CommandLineParameters})
            .AsSingleton();
        service.Bind<IOpenSaveFile>().To<OpenSaveFileAdapter>();
    }

    private static void ConfigureNugetUpload(IBindableIocService service)
    {
        service.AddConfigurationSources(i => i.AddUserSecrets<Startup>());
        service.Bind<IPackagePublishOperation>().To<NugetPublishOperation>().AsSingleton();
    }

    private void SetIcon(RootNavigationWindow arg) => 
        arg.SetWindowIconFromResource("WebDashboard", "RootWindows/app.ico");
}

