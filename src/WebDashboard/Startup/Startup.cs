using System;
using System.Windows;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RootWindows;
using Melville.WpfIocMvvm.StartupBases;
using WebDashboard.Views;

namespace WebDashboard.Startup
{
    public class Startup2 : StartupBase
    {
        [STAThread]
        public static int Main(string[] args)
        {
            ApplicationRootImplementation.Run(new Startup2(args));
            return 0;
        }

        public Startup2(string[] commandLineParameters) : base(commandLineParameters)
        {
        }

        protected override void RegisterWithIocContainer(IBindableIocService service)
        {
            service.AddLogging();
            // Root Window
            service.Bind<INavigationWindow>().To<NavigationWindow>().AsSingleton();
            service.Bind<RootNavigationWindow>().And<Window>().And<IRootNavigationWindow>()
                .ToSelf().WrapWith(SetIcon).AsSingleton();
            service.Bind<IHomeViewModel>().To<FileLoadViewModel>();
            
            // System Services
            service.Bind<IStartupData>().To<StartupData>()
                .WithParameters(new object[] {CommandLineParameters})
                .AsSingleton();
            service.Bind<IOpenSaveFile>().To<OpenSaveFileAdapter>();
        }

        private RootNavigationWindow SetIcon(RootNavigationWindow arg)
        {
            arg.SetWindowIconFromResource("WebDashboard", "RootWindows/app.ico");
            return arg;
        }
    }
}