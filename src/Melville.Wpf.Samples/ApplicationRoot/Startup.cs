using System;
using System.Windows.Data;
using Melville.IOC.IocContainers;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.Wpf.DiParameterSources;
using Serilog;

namespace Melville.Wpf.Samples.ApplicationRoot
{
    public sealed class Startup
    {
        public static IIocService CompositionRoot()
        {
            var ret = new IocContainer();
            new Startup(ret).Configure();
            return ret;
        }

        private readonly IocContainer service;

        private Startup(IocContainer service)
        {
            this.service = service;
        }

        private void Configure()
        {
            InjectThreadSafeCollectionMutex();
            RegisterInfrastructure();
        }
        
        private void InjectThreadSafeCollectionMutex()
        {
            ThreadSafeCollectionBuilder.SetFixupHook(BindingOperations.EnableCollectionSynchronization);
        }


        private void RegisterInfrastructure()
        {
            service.Bind<IDIIntegration>().To<DiBridge>();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            service.Bind<ILogger>().ToConstant(Log.Logger);
        }

    }
}