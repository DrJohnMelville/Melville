using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public sealed class ScopedItemTest
{
    private readonly IocContainer sut = new IocContainer();

    [Fact]
    public void ScopedItem()
    {
        sut.Bind<SimpleObjectImplementation>().To<SimpleObjectImplementation>().AsScoped();
        var s1 = sut.CreateScope();
        var o1 = s1.Get<SimpleObjectImplementation>();
        var o2 = s1.Get<SimpleObjectImplementation>();
        Assert.Equal(o1, o2);
        var s2 = sut.CreateScope();
        var o3 = s2.Get<SimpleObjectImplementation>();
        var o4 = s2.Get<SimpleObjectImplementation>();
        Assert.Equal(o3, o4);
        Assert.NotEqual(o1, o3);
    }

    [Fact]
    public void NestedScopesSearchEntireChain()
    {
        sut.Bind<SimpleObjectImplementation>().ToSelf().AsScoped();
        var innerScope = sut.CreateScope();
        var outerScope = innerScope.CreateScope();

        var innerObject = innerScope.Get<SimpleObjectImplementation>();
        var outerObject = outerScope.Get<SimpleObjectImplementation>();
        innerObject.Should().BeSameAs(outerObject);
    }

    [Fact]
    public void ScopesArePerBindingNotPerObjectType()
    {
        sut.Bind<SimpleObjectImplementation>().ToSelf().AsScoped();
        sut.Bind<SimpleObjectImplementation>().ToSelf().AsScoped();
        var innerScope = sut.CreateScope();
        var a = innerScope.Get<IList<SimpleObjectImplementation>>();
        var b = innerScope.Get<IList<SimpleObjectImplementation>>();
        a.Should().HaveCount(2);
        a.Zip(b, object.ReferenceEquals).All(x => x).Should().BeTrue();
        a[0].Should().NotBeSameAs(a[1]);
    }

    [Fact]
    public void NestedOuterScopeDoesNotInfluenceInner()
    {
        sut.Bind<SimpleObjectImplementation>().ToSelf().AsScoped();
        var innerScope = sut.CreateScope();
        var outerScope = innerScope.CreateScope();

        var outerObject = outerScope.Get<SimpleObjectImplementation>();
        var innerObject = innerScope.Get<SimpleObjectImplementation>();
        innerObject.Should().NotBeSameAs(outerObject);
    }

    [Fact]
    public void TransientObjectsInDifferentScopesPickUpTheRightScopedDependencies()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>().AsScoped();
        sut.Bind<SecondaryObject>().ToSelf();
        var scope1 = sut.CreateScope();
        var scope2 = sut.CreateScope();
        var inner1 = scope1.Get<ISimpleObject>();
        var inner2 = scope2.Get<ISimpleObject>();
        inner1.Should().NotBe(inner2);
        scope1.Get<SecondaryObject>().SimpleObject.Should().Be(inner1);
        scope2.Get<SecondaryObject>().SimpleObject.Should().Be(inner2);

        scope1.Get<SecondaryObject>().Should().NotBeSameAs(scope1.Get<SecondaryObject>());
    }

    [Fact]
    public void ObjectsCreatedInParentContainedRecordInProperScopes()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>().AsScoped();
        sut.Bind<SecondaryObject>().ToSelf();
        var scope1 = sut.CreateScope();
        var scope2 = sut.CreateScope();
        var outer1 = scope1.Get<SecondaryObject>();
        var outer2 = scope2.Get<SecondaryObject>();
        outer1.SimpleObject.Should().Be(scope1.Get<ISimpleObject>());
        outer2.SimpleObject.Should().Be(scope2.Get<ISimpleObject>());
        outer1.SimpleObject.Should().NotBe(outer2.SimpleObject);
    }

}