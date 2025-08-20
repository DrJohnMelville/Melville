using FluentAssertions;
using Melville.IOC.IocContainers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
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
        private bool Same => a == b; // ust the parameters to avoid a warning
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

    [Theory]
    [InlineData("Hello", 0)]
    [InlineData(null, 1)]
    public void IncludeParams(object? item, int count)
    {
        ioc.Bind<IA>().To<Aimpl>().WhenParameterHasValue("Hello");
        object[] parameters = item is null ? new object[] { } : new object[] { item };
        new IocServiceVerifier(ioc).UncreatableTypes(ioc, parameters).Should().HaveCount(count);
    }

    [Theory]
    [InlineData("Abab", 3)]
    [InlineData("Compo", 2)]
    [InlineData("I.$", 1)]
    [InlineData(".*", 0)]
    public void IfNotNamedTest(string filter, int count)
    {
        new IocServiceVerifier(typeof(Composite), typeof(IA), typeof(IB))
            .IfNotNamed(filter)
            .UncreatableTypes(ioc).Should().HaveCount(count);
    }

    [Theory]
    [InlineData("Abab", 0)]
    [InlineData("Compo", 1)]
    [InlineData("I.$", 2)]
    [InlineData(".*", 3)]
    public void IfNamed(string filter, int count)
    {
        new IocServiceVerifier(typeof(Composite), typeof(IA), typeof(IB))
            .IfNamed(filter)
            .UncreatableTypes(ioc).Should().HaveCount(count);
    }
}