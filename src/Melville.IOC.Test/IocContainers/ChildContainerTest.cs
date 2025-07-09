using System;
using FluentAssertions;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ChildContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public class ChildContainerTest
{
    private class Dep{}
    private interface IBO { Dep Dep { get; }}
    private record BO1 (Dep Dep): IBO{ }

    private record BO2(Dep Dep) : IBO;
        
    private readonly IocContainer sut = new IocContainer();

    private record DisposableBO(Dep Dep) : IBO, IDisposable
    {
        public bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public void Dispose() => IsDisposed = true;
    }

    [Fact]
    public void ChildContainterResolve()
    {
        sut.Bind<Dep>().ToSelf().AsSingleton();
        var c1 = sut.Get<ChildContainer>();
        c1.Bind<IBO>().To<BO1>();
        var c2 = sut.Get<ChildContainer>();
        c2.Bind<IBO>().To<BO2>();

        var v1 = c1.Get<IBO>();
        var v2 = c2.Get<IBO>();
            
        Assert.True(v1 is BO1);
        Assert.True(v2 is BO2);
        Assert.Equal(v1.Dep, v2.Dep);
    }

    [Fact]
    public void ThreeLevelTree()
    {
        var intermed = new DisposableChildContainer(sut, sut);
        intermed.Bind<DisposableBO>().ToSelf().AsSingleton();
        var leaf1 = intermed.Get<DisposableChildContainer>();
        var leaf2 = intermed.Get<DisposableChildContainer>();

        var o1 = leaf1.Get<DisposableBO>();
        var o2 = leaf2.Get<DisposableBO>();

        o1.Should().BeSameAs(o2);

        leaf1.Dispose();
        o2.IsDisposed.Should().BeFalse();
        o1.IsDisposed.Should().BeFalse();

        intermed.Dispose();
        o2.IsDisposed.Should().BeTrue();
        o1.IsDisposed.Should().BeTrue();

    }
    [Fact]
    public void ChildScopeContainterResolve()
    {
        sut.Bind<Dep>().ToSelf().AsSingleton();
        var c1 = sut.CreateScope().Get<ChildContainer>();
        c1.Bind<IBO>().To<BO1>();
        var c2 = sut.Get<ChildContainer>();
        c2.Bind<IBO>().To<BO2>();

        var v1 = c1.Get<IBO>();
        var v2 = c2.Get<IBO>();
            
        Assert.True(v1 is BO1);
        Assert.True(v2 is BO2);
        Assert.Equal(v1.Dep, v2.Dep);
    }
    [Fact]
    public void ManyNestedConstainers()
    {
        sut.Bind<Dep>().ToSelf().AsSingleton();
        var c1 = sut.Get<ChildContainer>();
        for (int i = 0; i < 10; i++)
        {
            c1 = c1.Get<ChildContainer>();
        }
        c1.Bind<IBO>().To<BO1>();

        var v1 = c1.Get<IBO>();
            
        Assert.True(v1 is BO1);
        Assert.Equal(sut.Get<Dep>(), v1.Dep);
    }

    public class DisposableSingleton : IDisposable
    {
        public bool Disposed { get; set; }
        public void Dispose()
        {
            Disposed = true;
        }
    }

    [Fact]
    public void CanCreateDisposableSingleton()
    {
        sut.Bind<DisposableChildContainer>().ToSelf().DisposeIfInsideScope();
        var c1 = sut.Get<DisposableChildContainer>();
        c1.Bind<DisposableSingleton>().ToSelf().AsSingleton();
        var obj = c1.Get<DisposableSingleton>();
        Assert.False(obj.Disposed);
        c1.Dispose();
        Assert.True(obj.Disposed);
    }

}