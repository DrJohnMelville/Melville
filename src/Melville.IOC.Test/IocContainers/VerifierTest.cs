using FluentAssertions;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public class VerifierTest
{
    private interface IA { }
    private interface IB { }
    private class Aimpl : IA { }
    private class Bimpl: IB{}
    private class Composite(IA a, IB b)
    {
    }

    private readonly IocContainer ioc = new();

    [Fact]
    public void CanCreateConcreteClasses()
    {
        new IocServiceVerifier(typeof(Aimpl)).VerifyCreatable(ioc);
        new IocServiceVerifier(typeof(Bimpl)).VerifyCreatable(ioc);
    }

    [Fact]
    public void CanFailToCreadDependantInterface()
    {
        new IocServiceVerifier(typeof(Composite)).Invoking(i => i.VerifyCreatable(ioc))
            .Should().Throw<IocException>();
    }

    [Fact]
    public void FailOnIocTest()
    {
        ioc.Bind<IA>().To<Aimpl>();
        ioc.Bind<Composite>().ToSelf();
        new IocServiceVerifier(ioc).UncreatableTypes(ioc).Should().HaveCount(1);
    }
}