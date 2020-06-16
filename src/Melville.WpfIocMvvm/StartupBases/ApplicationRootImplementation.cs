using System.Windows;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.RootWindows;
using Melville.WpfIocMvvm.IocBridges;

namespace Melville.WpfIocMvvm.StartupBases
{
    public interface IHomeViewModel{}
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
            var ioc = startUp.Create();
            var app = Application(ioc);
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
            if (ret.DataContext is INavigationWindow window &&
                root.CanGet<IHomeViewModel>())
            {
                window.NavigateTo(root.Get<IHomeViewModel>());
            }
            return ret;
        }
    }
}