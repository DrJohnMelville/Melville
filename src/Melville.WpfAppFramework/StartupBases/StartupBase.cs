using System;
using Melville.IOC.IocContainers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Melville.WpfAppFramework.StartupBases;

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
        AddLoggingIfNeeded(service);
        return service;
    }

    private void AddLoggingIfNeeded(IocContainer service)
    {
        if (service.CanGet(typeof(ILogger))) return;
        var factory = LoggerFactory.Create(b =>
        {
            b.AddDebug();
        });
        service.Bind<ILoggerFactory>().ToConstant(factory).DisposeIfInsideScope();
        service.BindGeneric(typeof(ILogger<>), typeof(Logger<>));
        service.Bind<ILogger>().ToMethod((ILoggerFactory f) => f.CreateLogger(""));
    }

    protected abstract void RegisterWithIocContainer(IBindableIocService service);
}

public static class WpfServiceBindings
{
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