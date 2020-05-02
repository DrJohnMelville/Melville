using System;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.RootWindows;
using WebDashboard.Views;

namespace WebDashboard.Startup
{

    public static class ApplicationEntryPoint
    {
        [STAThread]
        public static int Main(string[] args)
        {
            var ioc = CreateIocContainer(args);
            var application = SetupApplicationWithWindow(ioc);
            NavigateToDefaultView(ioc);
            application.Run();
            return 0;
        }

        private static IocContainer CreateIocContainer(string[] args)
        {
            var ioc = new IocContainer();
            ioc.Bind<IStartupData>().To<StartupData>().WithParameters(new object[]{args}).AsSingleton();
            Startup.SetupIoc(ioc);
            return ioc;
        }

        private static void NavigateToDefaultView(IocContainer ioc)
        {
            ioc.Get<INavigationWindow>().NavigateTo(ioc.Get<FileLoadViewModel>());
        }

        private static App SetupApplicationWithWindow(IocContainer ioc)
        {
            var application = ioc.Get<App>();
            application.InitializeComponent();
            application.AttachDiRoot(new DiBridge(ioc));
            var mainWin = ioc.Get<RootNavigationWindow>();
            mainWin.SetWindowIconFromResource("WebDashboard", "RootWindows/app.ico");
            application.MainWindow = mainWin;
            application.MainWindow.Show();
            return application;
        }
    }
}