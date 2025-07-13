using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FluentAssertions;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.TypeResolutionPolicy;

public class TupleToScopeResolutionPolicyTest
{
    public interface IDisp1 : IDisposable {             public int DisposeCount { get; set; } }
    public interface IDisp2 : IDisposable {             public int DisposeCount { get; set; } }

    public class Disp : IDisp1, IDisp2
    {
        public int DisposeCount { get; set; }
        public void Dispose()
        {
            DisposeCount++;
        }
    }
        
    private readonly Disp d1 = new Disp();
    private readonly Disp d2 = new Disp();
    private readonly IocContainer sut = new IocContainer();

    public TupleToScopeResolutionPolicyTest()
    {
        sut.Bind<IDisp1>().ToMethod(()=>d1).AsScoped();
        sut.Bind<IDisp2>().ToMethod(()=>d2).AsScoped();
    }

    [Fact]
    public void TuplePlusDisposeImpliesScope()
    {
        var (scope, d) = sut.Get<(IDisposable, IDisp1)>();
        Assert.NotNull(d);
        Assert.NotNull(scope);
        Assert.Equal(0, d.DisposeCount);
        scope.Dispose();
        Assert.Equal(1, d.DisposeCount);
    }

    [Fact]
    public async Task MultipleInOneTuple()
    {
        var (scope, d1, d2) = sut.Get<(IAsyncDisposable, IDisp1, IDisp2)>();
        Assert.Equal(0, d1.DisposeCount);
        Assert.Equal(0, d2.DisposeCount);
        await scope.DisposeAsync();
        Assert.Equal(1, d1.DisposeCount);
        Assert.Equal(1, d2.DisposeCount);
    }

    [Fact] public async Task MultipleInOneClassTuple()
    {
        var (scope, d1, d2) = sut.Get<(IAsyncDisposable, IDisp1, IDisp2)>();
        Assert.Equal(0, d1.DisposeCount);
        Assert.Equal(0, d2.DisposeCount);
        await scope.DisposeAsync();
        Assert.Equal(1, d1.DisposeCount);
        Assert.Equal(1, d2.DisposeCount);
    }

    private record HoldsScope((IDisposable Key, IDisp1 Value) item);

    public void SingletonsCanHoldScopeContainers()
    {
        sut.Bind<HoldsScope>().ToSelf().AsSingleton();
        var item = sut.CreateScope().Get<HoldsScope>();
        item.item.Value.DisposeCount.Should().Be(0);
        item.item.Key.Dispose();
        item.item.Value.DisposeCount.Should().Be(1);
    }
        
}