using System;
using Melville.IOC.IocContainers;
using Melville.Log.NamedPipeEventSink;
using Serilog;

namespace Melville.WpfIocMvvm.StartupBases
{
    public class StartupBase
    {
        public StartupBase(string[]? commandLineParameters = null)
        {
            CommandLineParameters = commandLineParameters ?? Array.Empty<String>();
        }

        protected string[] CommandLineParameters {get;}
        public IocContainer Create()
        {
            var service = new IocContainer();
            RegisterWithIocContainer(service);
            return service;
        }

        protected virtual void RegisterWithIocContainer(IBindableIocService service)
        {
            
        }
    }

    public static class WpfServiceBindings
    {
        public static void AddLogging(this IBindableIocService service)
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