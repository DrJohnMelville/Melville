using System;
using System.Windows.Media.Imaging;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ChildContainers;
using Melville.Log.Viewer.HomeScreens;
using Melville.Log.Viewer.NamedPipeServers;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.RootWindows;

namespace Melville.Log.Viewer.ApplicationRoot
{
    public class ApplicationEntry
    {
        [STAThread]
        public static int Main(string[] commandLineArgs)
        {
            var compositionRoot = new Startup().CompositionRoot();
            compositionRoot.Get<PipeListener>().Start();
            Application(compositionRoot).Run();
            compositionRoot.Get<IShutdownMonitor>().InitiateShutdown();
            return 0;
        }

        private static App Application(IIocService root)
        {
            var application = root.Get<App>();
            application.AttachDiRoot(new DiBridge(root));
            application.InitializeComponent();
            application.MainWindow = VisibleMainWindow(root);
            return application;
        }

        private static RootNavigationWindow VisibleMainWindow(IIocService root)
        {
            var ret = root.Get<RootNavigationWindow>();
            ret.SetWindowIconFromResource("Melville.Log.Viewer", "ApplicationRoot/app.ico");
            ret.Show();
            ((INavigationWindow)ret.DataContext).NavigateTo(root.Get<HomeScreenViewModel>());
            return ret;
        }

    }
}