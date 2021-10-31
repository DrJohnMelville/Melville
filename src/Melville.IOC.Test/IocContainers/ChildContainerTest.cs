using System;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ChildContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public class ChildContainerTest
{
    private class Dep{}
    private interface IBO { Dep Dep { get; }}
    private class BO1: IBO{
        public BO1(Dep dep)
        {
            Dep = dep;
        }

        public Dep Dep { get; }
    }

    private class BO2 : IBO
    {
        public BO2(Dep dep)
        {
            Dep = dep;
        }

        public Dep Dep { get; }
    }
        
    private readonly IocContainer sut = new IocContainer();

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