using System.Collections.Generic;
using FluentAssertions;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.TypeResolutionPolicy;

public class EnumerateMultipleBindingsTest
{
    private interface IItem
    {
        string Value { get; }
    }
    private record Item(string Value): IItem;
    private readonly IocContainer sut = new ();

    [Fact]
    public void ResolveEmptyList()
    {
        var result = sut.Get<IEnumerable<Item>>();
        result.Should().BeEmpty();
    }

    [Fact]
    public void ResolveSingleItemList()
    {
        sut.Bind<IItem>().ToConstant(new Item("Hello"));
        var result = sut.Get<IEnumerable<IItem>>();
        result.Should().ContainSingle().Which.Value.Should().Be("Hello");
    }

    [Fact]
    public void ResolveToCreateSingleItem()
    {
        sut.Bind<IItem>().To<Item>().WithParameters("Hello");
        var result = sut.Get<IEnumerable<IItem>>();
        result.Should().ContainSingle().Which.Value.Should().Be("Hello");
    }
}