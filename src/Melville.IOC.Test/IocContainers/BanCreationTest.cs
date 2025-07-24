using FluentAssertions;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public class BanCreationTest
{
    private readonly IocContainer sut = new IocContainer();

    [Fact]
    public void BanOverridesPruior()
    {
        sut.Bind<int>().ToConstant(3);
        sut.Bind<int>().Prohibit();
        sut.CanGet<int>().Should().BeFalse();
        sut.Get(typeof(int)).Should().BeNull();
    }

    [Fact]
    public void CanBanConditionally()
    {
        sut.Bind<int>().ToConstant(12);
        sut.Bind<int>().Prohibit().WhenParameterHasValue(2);
        sut.CanGet<int>(2).Should().BeFalse();
        sut.CanGet<int>(3).Should().BeTrue();
        sut.CanGet<int>().Should().BeTrue();
    }
}