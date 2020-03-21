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
        [InlineData(DisposalState.DisposalDone)]
        [InlineData(DisposalState.DisposeOptional)]
        public void ForbidDisposalDoesNotThrowInGlobalScope(DisposalState state)
        {
            RegisterDisposeType(state);
            Assert.NotNull(sut.Get<Disposable>()); // should not throw
        }

        private void RegisterDisposeType(DisposalState state)
        {
            var stem = sut.Bind<Disposable>().ToSelf();
            if (state == DisposalState.DisposalDone)
            {
                stem.DoNotDispose();
            }
            else
            {
                stem.DisposeIfInsideScope();
            }
        }

        [Theory]
        [InlineData(DisposalState.DisposalDone, 0)]
        [InlineData(DisposalState.DisposeOptional, 1)]
        public void ScopesIgnoreForbiddenDisposal(DisposalState state, int disposes)
        {
            RegisterDisposeType(state);
            var scope = sut.CreateScope();
            var obj = scope.Get<Disposable>();
            scope.Dispose();
            Assert.Equal(disposes, obj.DisposeCount);
        }
        
        [Theory]
        [InlineData(DisposalState.DisposalDone, 0)]
        [InlineData(DisposalState.DisposeOptional, 1)]
        public async Task OnlyTheInnermostScopeDisposes(DisposalState state, int disposes)
        {
            RegisterDisposeType(state);
            var outer = sut.CreateScope();
            var scope = outer.CreateScope();
            var obj = scope.Get<Disposable>();
            await scope.DisposeAsync();
            Assert.Equal(disposes, obj.DisposeCount);
            outer.Dispose();
            Assert.Equal(disposes, obj.DisposeCount);
        }
        
        // createmany needs to dispose of all ihe instances.
    }
}