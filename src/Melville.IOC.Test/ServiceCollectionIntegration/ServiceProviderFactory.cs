using System;
using FluentAssertions;
using Melville.IOC.AspNet.RegisterFromServiceCollection;
using Melville.IOC.IocContainers;
using Melville.IOC.Test.IocContainers;
using Melville.IOC.TypeResolutionPolicy;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ServiceCollection = Melville.IOC.AspNet.RegisterFromServiceCollection.ServiceCollection;

namespace Melville.IOC.Test.ServiceCollectionIntegration;

public class ServiceProviderFactory
{
    private readonly IServiceProviderFactory<IocContainer> sut = new MelvilleServiceProviderFactory(false);

    [Fact]
    public void Integration()
    {
        var sc = new ServiceCollection();
        sc.AddSingleton("Foo Bar");
        var builder = sut.CreateBuilder(sc);
        Assert.NotNull(builder);
        var provider = sut.CreateServiceProvider(builder);
        Assert.Equal("Foo Bar", provider.GetRequiredService<string>());
    }

    [Fact]
    public void Scoped()
    {
        var sc = new ServiceCollection();
        sc.AddScoped<ISimpleObject, SimpleObjectImplementation>();
        var prov = sut.CreateServiceProvider(sut.CreateBuilder(sc));
        using (var scope = prov.CreateScope())
        {
            Assert.NotNull(scope.ServiceProvider.GetService<ISimpleObject>());               
        }
    }

    [Fact]
    public void FailedBindingIsNull()
    {
        var failCount = 0;

        void OnFailRequestPolicyOnBindingFailed(object? sut, FailedRequestEventArgs e) => failCount++;

        FailRequestPolicy.BindingFailed += OnFailRequestPolicyOnBindingFailed;
        var prov = sut.CreateServiceProvider(sut.CreateBuilder(new ServiceCollection()));
        prov.GetService(typeof(ISimpleObject)).Should().BeNull();
        FailRequestPolicy.BindingFailed -= OnFailRequestPolicyOnBindingFailed;
        failCount.Should().Be(1);
    }
    [Fact]
    public void FailedSecondaryBindingIsNull()
    {
        var prov = sut.CreateServiceProvider(sut.CreateBuilder(new ServiceCollection()));
        prov.GetService(typeof(SecondaryObject)).Should().BeNull();
    }
    [Fact]
    public void FailedRequiredBindingThrows()
    {
        var prov = sut.CreateServiceProvider(sut.CreateBuilder(new ServiceCollection()));
        prov.Invoking(i=>i.GetRequiredService(typeof(ISimpleObject))).Should().
                Throw<InvalidOperationException>();
    }

    [Fact]
    public void ServiceProviderBindsIServiceProvider()
    {
        var sc = new ServiceCollection();
        var prov = sut.CreateServiceProvider(sut.CreateBuilder(sc));
        Assert.NotNull(prov.GetService<IServiceProvider>());            
    }

    [Fact]
    public void BindWithCustomAction()
    {
        var sut2 = new MelvilleServiceProviderFactory(false, i => i.Bind<string>().ToConstant("Hello World"));
        var sp = sut2.CreateServiceProvider(sut2.CreateBuilder(new ServiceCollection()));
        Assert.Equal("Hello World", sp.GetRequiredService<string>());
    }
}