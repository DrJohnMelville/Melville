using System;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.RootWindows;
using Melville.Wpf.Samples.SampleTreeViewDisplays;

namespace Melville.Wpf.Samples.ApplicationRoot
{
    public class ApplicationEntry
    {
        [STAThread]
        public static int Main(string[] commandLineArgs)
        {
            Application(Startup.CompositionRoot()).Run();
            return 0;
        }

        private static App Application(IIocService root)
        {
            var application = root.Get<App>();
            application.InitializeComponent();
            application.MainWindow = VisibleMainWindow(root);
            return application;
        }

        private static RootNavigationWindow VisibleMainWindow(IIocService root)
        {
            var ret = root.Get<RootNavigationWindow>();
            ret.Show();
            ((INavigationWindow)ret.DataContext).NavigateTo(root.Get<SamplesTreeViewModel>());
            return ret;
        }

    }
}