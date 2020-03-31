using System;
using System.Windows.Data;
using Melville.IOC.IocContainers;
using Melville.Log.NamedPipeEventSink;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.Wpf.Samples.ScopedMethodCalls;
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
            RegisterInfrastructure();

            service.Bind<DisposableDependency>().ToSelf().AsScoped();
        }
        
        private void RegisterInfrastructure()
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.NamedPipe()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            service.Bind<ILogger>().ToConstant(Serilog.Log.Logger);
        }

    }
}