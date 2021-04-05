using System;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.BindingSources;
using Melville.IOC.TypeResolutionPolicy;
using Melville.Log.NamedPipeEventSink;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Melville.WpfAppFramework.StartupBases
{
    public abstract class StartupBase
    {
        public StartupBase(string[]? commandLineParameters = null)
        {
            CommandLineParameters = commandLineParameters ?? Array.Empty<String>();
        }

        protected string[] CommandLineParameters {get;}
        public virtual IIocService Create()
        {
            var service = new IocContainer();
            RegisterWithIocContainer(service);
            return service;
        }

        protected abstract void RegisterWithIocContainer(IBindableIocService service);
    }

    public static class WpfServiceBindings
    {
        public static IActivationOptions<ILogger> AddLogging(this IBindableIocService service)
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.NamedPipe()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            return service.Bind<ILogger>().ToConstant(Serilog.Log.Logger);
        }

        public static IActivationOptions<IConfigurationRoot> AddConfigurationSources(this IBindableIocService service, Action<IConfigurationBuilder> build)
        {
            var builder = new ConfigurationBuilder();
            build(builder);
            return service.Bind<IConfigurationRoot>().ToConstant(builder.Build()).DisposeIfInsideScope();
        }

        public static IActivationOptions<TSource> InitializeFromConfiguration<TSource>(
            this IActivationOptions<TSource> src, string key) => src.FixResult((item, req) => 
              req.IocService.Get<IConfigurationRoot>().GetSection(key).Bind(item));
        public static IActivationOptions<TSource> InitializeFromConfiguration<TSource>(
            this IActivationOptions<TSource> src, IConfiguration config, string key) => 
            src.InitializeFromConfiguration(config.GetSection(key));
        public static IActivationOptions<TSource> InitializeFromConfiguration<TSource>(
            this IActivationOptions<TSource> src, IConfiguration config) =>
            src.FixResult(i => config.Bind(i));
    }
}