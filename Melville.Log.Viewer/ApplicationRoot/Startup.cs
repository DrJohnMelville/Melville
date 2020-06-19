using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;
using Melville.Log.Viewer.HomeScreens;
using Melville.Log.Viewer.NamedPipeServers;
using Melville.MVVM.Wpf.RootWindows;
using Melville.WpfAppFramework.StartupBases;
using Microsoft.Extensions.Configuration;

namespace Melville.Log.Viewer.ApplicationRoot
{
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
            SetupPipeListener(service);
            SetupMainWindowContent(service);
        }

        private void SetupMainWindowContent(IBindableIocService service)
        {
            service.Bind<Application>().To<App>().FixResult(i => ((App) i).InitializeComponent()).AsSingleton();
            service.RegisterHomeViewModel<HomeScreenViewModel>();
            service.Bind<RootNavigationWindow>().And<Window>().And<IRootNavigationWindow>()
                .ToSelf().FixResult(SetIcon).AsSingleton();
        }

        private void SetIcon(RootNavigationWindow arg) => 
            arg.SetWindowIconFromResource("Melville.Log.Viewer", "ApplicationRoot/app.ico");

        private static void SetupPipeListener(IBindableIocService service)
        {
            service.Bind<IShutdownMonitor>().To<ShutdownMonitor>().AsSingleton();
            service.Bind<IPipeListener>().And<PipeListener>().To<PipeListener>().AsSingleton();
        }

        private static void SetupConfiguration(IBindableIocService service)
        {
            service.AddConfigurationSources(i => i.AddUserSecrets<Startup>());
            service.Bind<IList<TargetSite>>().To<List<TargetSite>>(ConstructorSelectors.DefaultConstructor)
                .InitializeFromConfiguration("Links")
                .AsSingleton();
        }
    }
}