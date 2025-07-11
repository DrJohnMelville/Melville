using System;
using System.Threading.Tasks;
using Melville.IOC.IocContainers;
using Moq;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public sealed class ObjectLifetimeTests
{
    private readonly IocContainer sut = new IocContainer();
        
    [Fact]
    public void TRansientObjectsAreDifferent()
    {
        sut.Bind<SimpleObjectImplementation>().To<SimpleObjectImplementation>();
        var o1 = sut.Get<SimpleObjectImplementation>();
        var o2 = sut.Get<SimpleObjectImplementation>();
        Assert.NotEqual(o1,o2);
    }
    [Fact]
    public void SingletonObjectIsSame()
    {
        sut.Bind<SimpleObjectImplementation>().To<SimpleObjectImplementation>().AsSingleton();
        var o1 = sut.Get<SimpleObjectImplementation>();
        var o2 = sut.Get<SimpleObjectImplementation>();
        Assert.Equal(o1,o2);
    }

    [Fact]
    public void PropogateScope()
    {
        var scope = sut.CreateScope();
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>().AsScoped();
        sut.Bind<SecondaryObject>().ToSelf();
        Assert.NotNull(scope.Get<SecondaryObject>());
    }
    [Fact]
    public void CannotMakeScopedIntoSingleton()
    {
        Assert.Throws<IocException>(()=>sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>().AsScoped().AsSingleton());
    }
    [Fact]
    public void CannotMakeSingletonIntoScoped()
    {
        Assert.Throws<IocException>(()=>sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>().AsSingleton().AsScoped());
    }
    [Fact]
    public void ForbidScopedObjectsInStaticContext()
    {
        var scope = sut.CreateScope();
        sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>().AsScoped();
        sut.Bind<SecondaryObject>().ToSelf().AsSingleton();
        Assert.Throws<IocException>(() => scope.Get<SecondaryObject>());
    }
    //need to maeke scoped disposable.
    [Fact]
    public async Task AsyncDisposeScopedItem()
    {
        var dispMock = new Mock<IDisposable>();
        sut.Bind<IDisposable>().ToMethod(()=>dispMock.Object).AsScoped();
        await using (var scope = sut.CreateScope())
        {
            scope.Get<IDisposable>();
        }
        dispMock.Verify(i=>i.Dispose(), Times.Once);
    }
    [Fact]
    public async Task AsyncDisposeAsyncScopedItem()
    {
        var dispMock = new Mock<IAsyncDisposable>();
        sut.Bind<IAsyncDisposable>().ToMethod(()=>dispMock.Object).AsScoped();
        await using (var scope = sut.CreateScope())
        {
            scope.Get<IAsyncDisposable>();
        }
        dispMock.Verify(i=>i.DisposeAsync(), Times.Once);
    }
    [Fact]
    public void DisposeScopedItem()
    {
        var dispMock = new Mock<IDisposable>();
        sut.Bind<IDisposable>().ToMethod(()=>dispMock.Object).AsScoped();
        using (var scope = sut.CreateScope())
        {
            scope.Get<IDisposable>();
        }
        dispMock.Verify(i=>i.Dispose(), Times.Once);
    }
    [Fact]
    public void DisposeAsyncScopedItem()
    {
        var dispMock = new Mock<IAsyncDisposable>();
        sut.Bind<IAsyncDisposable>().ToMethod(()=>dispMock.Object).AsScoped();
        using (var scope = sut.CreateScope())
        {
            scope.Get<IAsyncDisposable>();
        }
        dispMock.Verify(i=>i.DisposeAsync(), Times.Once);
    }
    [Fact]
    public async Task OnlyDisposeOnce()
    {
        var dispMock = new Mock<IDisposable>();
        sut.Bind<IDisposable>().ToMethod(()=>dispMock.Object).AsScoped();
        await using (var scope = sut.CreateScope())
        {
            scope.Get<IDisposable>();
            await scope.DisposeAsync();
        }
        dispMock.Verify(i=>i.Dispose(), Times.Once);
    }
}