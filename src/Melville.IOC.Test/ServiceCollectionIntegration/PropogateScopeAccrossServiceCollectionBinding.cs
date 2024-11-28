using System;
using FluentAssertions;
using Melville.IOC.AspNet.RegisterFromServiceCollection;
using Melville.IOC.IocContainers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ServiceCollection = Microsoft.Extensions.DependencyInjection.ServiceCollection;

namespace Melville.IOC.Test.ServiceCollectionIntegration;

public class PropogateScopeAccrossServiceCollectionBinding()
{
    private class ScopedInner () {}

    private class ScopedOuter(IServiceProvider coll)
    {
        private ScopedInner item = coll.GetRequiredService<ScopedInner>();
    }

    // This tests correct behavior in response to an antipattern that appears in
    // blazor.  ScopedOuter should just depend on scopedInner, but instead it
    // depends on IServiceProvider (the service locator antipattern) and then
    // immediately requests a ScopedInner.  This test makes sure that the 
    // IServiceProvider provided to ScopedOuter stays inside the scope.

    [Fact]
    public void Resolve()
    {
        var sc = new ServiceCollection();
        sc.AddScoped<ScopedInner>();
        sc.AddScoped<ScopedOuter>();

        var prov = new MelvilleServiceProviderFactory(true);
        var sp = prov.CreateServiceProvider(prov.CreateBuilder(sc));

        var scopeProv = sp.CreateScope().ServiceProvider;
        scopeProv.GetService(typeof(ScopedOuter)).Should().BeOfType<ScopedOuter>();
    }

    private class ScopedOuterFactory(Func<ScopedInner> makeInner)
    {
        private ScopedInner inner = makeInner();
    }

    // lets try the same thing using our factory interfaces.
    [Fact]
    public void UsingFactories()
    {
        var ioc = new IocContainer();
        ioc.Bind<ScopedInner>().ToSelf().AsScoped();
        ioc.Bind<ScopedOuterFactory>().ToSelf().AsScoped();

        ioc.CreateScope().Get<ScopedOuterFactory>()
            .Should().BeOfType<ScopedOuterFactory>();
    }

}