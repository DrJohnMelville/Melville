using System;
using System.Windows;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RootWindows;
using Melville.WpfAppFramework.StartupBases;

namespace WebDashboard.Startup
{
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
            service.AddLogging();
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

        private void SetIcon(RootNavigationWindow arg) => 
            arg.SetWindowIconFromResource("WebDashboard", "RootWindows/app.ico");
    }
}