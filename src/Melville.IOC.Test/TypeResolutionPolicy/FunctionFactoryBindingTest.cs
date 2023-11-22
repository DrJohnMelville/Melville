using System;
using Melville.IOC.IocContainers;
using Melville.IOC.TypeResolutionPolicy;
using Xunit;

namespace Melville.IOC.Test.TypeResolutionPolicy;

public class FunctionFactoryBindingTest
{
    private readonly IocContainer sut = new IocContainer();
    public class Simple { }

    [Fact]
    public void NoArguments()
    {
        var f = sut.Get<Func<Simple>>();
        Assert.NotNull(f());
    }
    [Fact]
    public void OneArgument()
    {
        var f = sut.Get<Func<int, Simple>>();
        Assert.NotNull(f(1));
    }
    [Fact]
    public void ManyArguments()
    {
        var f = sut.Get<Func<int, string, int, int, int, int, int, int, int, int, int, int, int, int,
            int, int, Simple>>();
        Assert.NotNull(f(1, "2", 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 15, 16));
    }

    public class HasThreeArguments
    {
        public int Int { get; }
        public int Int2 { get; }
        public Simple Simple { get; }

        public HasThreeArguments(int i, int int2, Simple simple)
        {
            Int = i;
            Int2 = int2;
            Simple = simple;
        }
    }

    [Fact]
    public void InitializeWithParameters()
    {
        var fact = sut.Get<Func<int, int, HasThreeArguments>>();
        var item = fact(1, 2);
        Assert.Equal(1, item.Int);
        Assert.Equal(2, item.Int2);
        Assert.NotNull(item.Simple);
    }

    [Fact]
    public void BindWithParameters()
    {
        sut.Bind<HasThreeArguments>().To<HasThreeArguments>().WithParameters(1, 2);
        var item = sut.Get<HasThreeArguments>();
        Assert.Equal(1, item.Int);
        Assert.Equal(2, item.Int2);
        Assert.NotNull(item.Simple);
        var item2 = sut.Get<HasThreeArguments>();
        Assert.Equal(1, item2.Int);
        Assert.Equal(2, item2.Int2);
        Assert.NotNull(item2.Simple);
    }

    [Fact]
    public void SpecifyParametersToGet()
    {
        var item = sut.Get<HasThreeArguments>(1,2);
        Assert.Equal(1, item.Int);
        Assert.Equal(2, item.Int2);
        Assert.NotNull(item.Simple);
    }

    [Fact]
    public void CanGetWithArguments()
    {
        Assert.False(sut.CanGet<HasThreeArguments>());
        Assert.True(sut.CanGet<HasThreeArguments>(1,2));
    }
}