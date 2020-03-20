using System.Windows;
using System.Windows.Data;
using Melville.IOC.IocContainers;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RootWindows;
using Serilog;
using Serilog.Events;

namespace WebDashboard.Startup
{
    public static class Startup
    {
        public static void SetupIoc(IocContainer service)
        {
            StaticListInitialization();
            
            ConfigureLog();

            
            service.Bind<IIocService>().ToConstant(service);

            // Root Window
            service.Bind<INavigationWindow>().To<NavigationWindow>().AsSingleton();
            service.Bind<RootNavigationWindow>().And<Window>().ToSelf().AsSingleton();
            
            // System Services
            service.Bind<IOpenSaveFile>().To<OpenSaveFileAdapter>();
        }

        private static void ConfigureLog()
        {
            Log
                .Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Dynamic(i => i.Trace(), LogEventLevel.Information)
                .CreateLogger();
        }

        private static void StaticListInitialization()
        {
            ThreadSafeCollectionBuilder.SetFixupHook(BindingOperations.EnableCollectionSynchronization);
        }
    }
}