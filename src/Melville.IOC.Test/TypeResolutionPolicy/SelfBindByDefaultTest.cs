﻿using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.Test.IocContainers;
using Melville.IOC.TypeResolutionPolicy;
using Xunit;

namespace Melville.IOC.Test.TypeResolutionPolicy;

public class SelfBindByDefaultTest
{
    private readonly IocContainer sut = new IocContainer();
    [Fact]
    public void SelfBindSimpleObject()
    {
        Assert.NotNull(sut.Get<SimpleObjectImplementation>());
    }

    [Fact]
    public void CanRemoveBindingPolicies()
    {
        sut.RemoveTypeResolutionPolicy<SelfBindByDefault>();
        Assert.False(sut.CanGet<SimpleObjectImplementation>());
    }
    public struct structWithConstructor
    {
        public structWithConstructor(SimpleObjectImplementation s)
        {
            I = 10;
        }

        public int I { get; }
    }
    [Fact]
    public void SelfBindValueType()
    {
        Assert.Equal(10, sut.Get<structWithConstructor>().I);
    }

    [Fact]
    public void SelfBindingIsDurable()
    {
        Assert.Null(ResolveFromCacheOnly());
        Assert.NotNull(sut.Get<SimpleObjectImplementation>());
        Assert.NotNull(ResolveFromCacheOnly());

    }

    private IActivationStrategy? ResolveFromCacheOnly()
    {
        return sut
            .TypeResolver
            .GetInstantiationPolicy<CachedResolutionPolicy>()?
            .ApplyResolutionPolicy(new RootBindingRequest(typeof(SimpleObjectImplementation), sut));
    }

    private abstract class AbsType();

    [Fact]
    public void CannotSelfBindAbstractType()
    {
        sut.AddTypeResolutionPolicyToEnd(new FailRequestPolicy());
        Assert.False(sut.CanGet<AbsType>());
        Assert.Null(sut.Get(typeof(AbsType)));
    }
}