using System;
using System.Threading.Tasks;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers
{
    public class DisposableIocServiceTest
    {
        public class Disposable : IDisposable
        {
            public int DisposeCount { get; private set; }

            public void Dispose() => DisposeCount++;
        }

        private readonly IocContainer sut = new IocContainer();

        [Fact]
        public void ThrowIfDisposableObjectWillNotBeDisposed()
        {
            Assert.Throws<IocException>(() => sut.Get<Disposable>());
        }
        [Fact]
        public void REgisterActivationStrategiesAsConstructor()
        {
            Assert.Throws<IocException>(() => sut.Get<Disposable>());
            Assert.Throws<IocException>(() => sut.Get<Disposable>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ForbidDisposalDoesNotThrowInGlobalScope(bool state)
        {
            RegisterDisposeType(state);
            Assert.NotNull(sut.Get<Disposable>()); // should not throw
        }

        private void RegisterDisposeType(bool allow)
        {
            var stem = sut.Bind<Disposable>().ToSelf();
            if (!allow)
            {
                stem.DoNotDispose();
            }
            else
            {
                stem.DisposeIfInsideScope();
            }
        }

        [Theory]
        [InlineData(false, 0)]
        [InlineData(true, 1)]
        public void ScopesIgnoreForbiddenDisposal(bool allow, int disposes)
        {
            RegisterDisposeType(allow);
            var scope = sut.CreateScope();
            var obj = scope.Get<Disposable>();
            scope.Dispose();
            Assert.Equal(disposes, obj.DisposeCount);
        }
        
        [Theory]
        [InlineData(false, 0)]
        [InlineData(true, 1)]
        public async Task OnlyTheInnermostScopeDisposes(bool allow, int disposes)
        {
            RegisterDisposeType(allow);
            var outer = sut.CreateScope();
            var scope = outer.CreateScope();
            var obj = scope.Get<Disposable>();
            await scope.DisposeAsync();
            Assert.Equal(disposes, obj.DisposeCount);
            outer.Dispose();
            Assert.Equal(disposes, obj.DisposeCount);
        }

        public class DisposeHolder:IDisposable
        {
            public DisposeHolder(IDisposable disp)
            {
                Disp = disp;
            }

            public IDisposable Disp {get;}
            public void Dispose()
            {
                Disp.Dispose();
            }
        }
        [Fact]
        public void ScopeDoesNotResusivelyDispose()
        {
            var scope = sut.CreateScope();
        //    var s2 = scope.Get<DisposeHolder>(scope);
            var s2 = scope.Get<IIocService>(scope);
            scope.Dispose();
            // should not overflow the stack
        }
    }
}