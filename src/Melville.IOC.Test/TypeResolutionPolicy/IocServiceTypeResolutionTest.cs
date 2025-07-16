using FluentAssertions;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ChildContainers;
using Xunit;

namespace Melville.IOC.Test.TypeResolutionPolicy;

public class IocServiceTypeResolutionTest
{
    private readonly IocContainer sut = new();

    [Fact]
    public void ResolveIocService()
    {
        sut.Get<IIocService>().Should().Be(sut);
    }
    [Fact]
    public void ResolveIocContainer()
    {
        sut.Get<IocContainer>().Should().Be(sut);
    }
    [Fact]
    public void ResolveBindableIocService()
    {
        sut.Get<IBindableIocService>().Should().Be(sut);
    }
    [Fact]
    public void ResolveChild()
    {

        // there is something funky here it is not accessing the inner child container.
        sut.Bind<DisposableChildContainer>().ToSelf().DoNotDispose();
        var child = sut.Get<DisposableChildContainer>();
        child.Get<IDisposableIocService>().Should().Be(child);
        child.Get<IIocService>().Should().Be(child);
        child.Get<IBindableIocService>().Should().Be(child);
        child.Get<IocContainer>().Should().Be(child);
    }

    [Fact]
    public void BindToBindingRequest()
    {
        sut.Get<IBindingRequest>().DesiredType.Should().Be(typeof(IBindingRequest));
    }
}