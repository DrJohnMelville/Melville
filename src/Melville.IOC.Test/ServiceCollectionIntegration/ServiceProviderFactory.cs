using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Melville.IOC.AspNet.RegisterFromServiceCollection;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.Debuggers;
using Melville.IOC.Test.IocContainers;
using Melville.IOC.TypeResolutionPolicy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using ServiceCollection = Melville.IOC.AspNet.RegisterFromServiceCollection.ServiceCollection;

namespace Melville.IOC.Test.ServiceCollectionIntegration;

public class ServiceProviderFactory
{
    private readonly IServiceProviderFactory<IocContainer> sut =
        new MelvilleServiceProviderFactory(false);

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
    public void IServiceProviderIsServiceTest()
    {
        var sc = new ServiceCollection();
        sc.AddTransient<ISimpleObject, SimpleObjectImplementation>();
        var prov = sut.CreateServiceProvider(sut.CreateBuilder(sc));

        var isService = prov.GetRequiredService<IServiceProviderIsService>();

        isService.IsService(typeof(ISimpleObject)).Should().BeTrue();
        isService.IsService(typeof(ISimpleObject2)).Should().BeFalse();
    }

    [Fact]
    public void HandleSpfDispose()
    {
        var sc = new ServiceCollection();
        var disposables = new List<Mock<IDisposable>>();
        sc.AddScoped<IDisposable>(i =>
        {
            var mock = new Mock<IDisposable>();
            disposables.Add(mock);
            return mock.Object;
        });
        var prov = sut.CreateServiceProvider(sut.CreateBuilder(sc));
        var scope = prov.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var v1 = scope.ServiceProvider.GetRequiredService<IDisposable>();
        var v2 = scope.ServiceProvider.GetRequiredService<IDisposable>();
        v1.Should().BeSameAs(v2);
        disposables.Should().HaveCount(1);
        disposables.ForEach(i => i.Verify(j => j.Dispose(), Times.Never));
        scope.Dispose();
        disposables.ForEach(i => i.Verify(j => j.Dispose(), Times.Once));
    }

    private record RequiresServiceProvider(IServiceProvider SP);
    private record TransitiveRequireServiceProvider(RequiresServiceProvider R);

    [Theory]
    [InlineData(typeof(RequiresServiceProvider))]
    [InlineData(typeof(TransitiveRequireServiceProvider))]
    public void CanUseSpecialTypesInASingleton(Type desiredType)
    {
        var sc = new ServiceCollection();
        sc.AddSingleton<RequiresServiceProvider>();
        sc.AddSingleton<TransitiveRequireServiceProvider>();
        var prov = sut.CreateServiceProvider(sut.CreateBuilder(sc));
        prov.GetService(desiredType);
    }

    [Fact]
    public void FailedBindingIsNull()
    {
        var debug = new Mock<IIocDebugger>();
        var builder = sut.CreateBuilder(new ServiceCollection());
        builder.Debugger = debug.Object;
        var prov = sut.CreateServiceProvider(builder);
        prov.GetService(typeof(ISimpleObject)).Should().BeNull();
        debug.Verify(i => i.ResolutionFailed(It.IsAny<IBindingRequest>()), Times.Once);
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


    [Fact]
    public void DependentObjectsFindTheOuterScopeServiceProvider()
    {
        var sc = new ServiceCollection();
        sc.AddTransient<RequiresServiceProvider>();
        var prov = sut.CreateServiceProvider(sut.CreateBuilder(sc));
        using var scope = prov.CreateScope();
        var dep = scope.ServiceProvider.GetRequiredService<RequiresServiceProvider>();
        dep.SP.Should().BeSameAs(scope);

    }
}