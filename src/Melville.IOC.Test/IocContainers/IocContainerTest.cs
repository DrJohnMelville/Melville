﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public interface ISimpleObject
{
}

public interface ISimpleObject2
{
}

public class SimpleObjectImplementation : ISimpleObject, ISimpleObject2
{
}

public record SecondaryObject(ISimpleObject SimpleObject);

public class IocContainerTest
{
    private readonly IocContainer sut = new IocContainer();

    [Fact]
    public void CreateWithParameter()
    {
        var parameter = new SimpleObjectImplementation();
        sut.Bind<SecondaryObject>().ToSelf().WithParameters(parameter);
        sut.Get<SecondaryObject>().SimpleObject.Should().Be(parameter);
        sut.CanGet<SecondaryObject>().Should().BeTrue();
    }
        
    [Fact]
    public void CreateSimpleObject()
    {
        sut.Bind<SimpleObjectImplementation>().To<SimpleObjectImplementation>();

        Assert.NotNull(sut.Get<SimpleObjectImplementation>());
    }

    [Fact]
    public void BindToInterface()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        Assert.True(sut.Get<ISimpleObject>() is SimpleObjectImplementation);
    }

    [Fact]
    public void BindMultipleInterfaces()
    {
        sut.Bind<ISimpleObject>()
            .And<SimpleObjectImplementation>()
            .And<ISimpleObject2>()
            .To<SimpleObjectImplementation>();
        Assert.True(sut.Get<ISimpleObject>() is SimpleObjectImplementation);
        Assert.True(sut.Get<SimpleObjectImplementation>() is SimpleObjectImplementation);
        Assert.True(sut.Get<ISimpleObject2>() is SimpleObjectImplementation);
    }

    [Fact]
    public void BindToConstant()
    {
        var single = new SimpleObjectImplementation();
        sut.Bind<ISimpleObject>().ToConstant(single);
        Assert.Equal(single, sut.Get<ISimpleObject>());
    }
        
    [Fact]
    public void Indirection()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        sut.Bind<SecondaryObject>().To<SecondaryObject>();
        var ret = sut.Get<SecondaryObject>();
        Assert.NotNull(ret);
        Assert.NotNull(ret.SimpleObject);
    }

    [Fact]
    public void BindUsingMethodWithIoc()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        sut.Bind<SecondaryObject>().ToMethod(i => new SecondaryObject(i.Get<ISimpleObject>()));
        var ret = sut.Get<SecondaryObject>();
        Assert.NotNull(ret.SimpleObject);
    }
    [Fact]
    public void BindUsingMethodWithIoc2()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        sut.Bind<SecondaryObject>().ToMethod((i,j) => new SecondaryObject(i.Get<ISimpleObject>()));
        var ret = sut.Get<SecondaryObject>();
        Assert.NotNull(ret.SimpleObject);
    }
    [Fact]
    public void BindUsingCustomMethodWithIoc()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        sut.Bind<SecondaryObject>().ToMethod(static (ISimpleObject i) => new SecondaryObject(i));
        var ret = sut.Get<SecondaryObject>();
        Assert.NotNull(ret.SimpleObject);
    }
    [Fact]
    public void BindUsingCustomMethodWithLocalIoc()
    {
        var holder = new LocalMethodHolder();
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        sut.Bind<SecondaryObject>().ToMethod(holder.Get);
        var ret = sut.Get<SecondaryObject>();
        Assert.NotNull(ret.SimpleObject);
    }

    private class LocalMethodHolder
    {
        public SecondaryObject Get(ISimpleObject is1) => new SecondaryObject(is1);
    }

    [Fact]
    public void CannotBindOpenGeneric()
    {
        Assert.Throws<IocException>(()=>sut.Get(typeof(IEnumerable<>)));
    }
    [Fact]
    public void BindingFailureMessage()
    {
        try
        {
            sut.Get<SecondaryObject>();
        }
        catch (Exception e)
        {
            e.Message.Should().StartWith("""
                Requested type: ISimpleObject
                [1, 1] ParameterBindingRequest ISimpleObject
                [1, 1] RootBindingRequest SecondaryObject
                
                Active Scopes:
                IocContainer(0x
                """);
            return;
        }
        Assert.Fail("should have thrown an exception");
    }

    [Fact]
    public void QueryIfAbleToCreate()
    {
        Assert.False(sut.CanGet<SecondaryObject>());
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        Assert.True(sut.CanGet<SecondaryObject>());
    }

    [Fact]
    public void QueryCanCreateFactory()
    {
        Assert.True(sut.CanGet<Func<SecondaryObject>>());
        Assert.True(sut.CanGet<Func<ISimpleObject,SecondaryObject>>());
    }

    public class HasDefaultParam
    {
        public int P1 { get; }

        public HasDefaultParam(int p1 = 30)
        {
            P1 = p1;
        }
    }
    [Fact]
    public void CanDoDefaultParams()
    {
        Assert.Equal(30, sut.Get<HasDefaultParam>().P1);
    }

    public class OpenGeneric<T>
    {
            
    }
    [Fact]
    public void TestName()
    {
        Assert.Throws<IocException>(()=>sut.Get(typeof(OpenGeneric<>)));
    }

    public class PickConstructorClass
    {
        public int Which { get; }

        public PickConstructorClass()
        {
            Which = 0;
        }
        public PickConstructorClass(ISimpleObject obj)
        {
            Which = 1;
        }
        public PickConstructorClass(ISimpleObject obj, ISimpleObject obj2)
        {
            Which = 2;
        }
    }

    [Fact]
    public void PickDefault()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        sut.Bind<PickConstructorClass>().To<PickConstructorClass>(ConstructorSelectors.DefaultConstructor);
        Assert.Equal(0, sut.Get<PickConstructorClass>().Which);
    }
    [Fact]
    public void PickOne()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        sut.Bind<PickConstructorClass>()
            .To<PickConstructorClass>(ConstructorSelectors.WithArgumentTypes<ISimpleObject>);
        Assert.Equal(1, sut.Get<PickConstructorClass>().Which);
    }
    [Fact]
    public void PickTwo()
    {
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
        sut.Bind<PickConstructorClass>()
            .To<PickConstructorClass>(ConstructorSelectors.WithArgumentTypes<ISimpleObject, ISimpleObject>);
        Assert.Equal(2, sut.Get<PickConstructorClass>().Which);
    }


}