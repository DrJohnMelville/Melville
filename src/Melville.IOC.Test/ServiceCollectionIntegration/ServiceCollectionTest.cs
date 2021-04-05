using Melville.IOC.AspNet.RegisterFromServiceCollection;
using Melville.IOC.IocContainers;
using Melville.IOC.Test.IocContainers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Melville.IOC.Test.ServiceCollectionIntegration
{
    public class ServiceCollectionTest
    {
        private readonly IServiceCollection source =
            new Melville.IOC.AspNet.RegisterFromServiceCollection.ServiceCollection();

        private readonly IocContainer sut = new IocContainer();

        [Fact]
        public void SimpleBind()
        {
            source.AddTransient<ISimpleObject2, SimpleObjectImplementation>();
            sut.BindServiceCollection(source);
            Assert.True(sut.Get<ISimpleObject2>() is SimpleObjectImplementation);
        }

        [Fact]
        public void SecondFormulation()
        {
            sut.BindServiceCollection(i => i.AddTransient<ISimpleObject2, SimpleObjectImplementation>());
            Assert.NotNull(sut.Get<ISimpleObject2>());
        }

        [Fact]
        public void BindToConstant()
        {
            var obj = new SimpleObjectImplementation();
            source.AddSingleton<ISimpleObject2>(obj);
            sut.BindServiceCollection(source);
            Assert.Equal(obj, sut.Get<ISimpleObject2>());
        }

        [Fact]
        public void BindToFunction()
        {
            // note this would be illegal in .net, but we will self bind SimpleObject
            source.AddTransient<ISimpleObject2>(isp => isp.GetService<SimpleObjectImplementation>());
            sut.BindServiceCollection(source);
            Assert.True(sut.Get<ISimpleObject2>() is SimpleObjectImplementation);
        }

        [Fact]
        public void Singleton()
        {
            source.AddSingleton<ISimpleObject2, SimpleObjectImplementation>();
            sut.BindServiceCollection(source);
            Assert.Equal(sut.Get<ISimpleObject2>(), sut.Get<ISimpleObject2>());
        }

        [Fact]
        public void ScopedBindingsAreUntested()
        {
            source.AddScoped<ISimpleObject2, SimpleObjectImplementation>();
            sut.BindServiceCollection(source);
            using var s1 = sut.CreateScope();
            using var s2 = sut.CreateScope();
            Assert.Equal(s1.Get<ISimpleObject2>(), s1.Get<ISimpleObject2>());
            Assert.Equal(s2.Get<ISimpleObject2>(), s2.Get<ISimpleObject2>());
            Assert.NotEqual(s1.Get<ISimpleObject2>(), s2.Get<ISimpleObject2>());
        }

        public interface IGeneric<T>
        {
        }

        public class Concrete<T> : IGeneric<T>
        {
        }

        [Fact]
        public void BindOpenGenerics()
        {
            source.AddTransient(typeof(IGeneric<>), typeof(Concrete<>));
            sut.BindServiceCollection(source);
            Assert.True(sut.Get<IGeneric<int>>() is Concrete<int>);

        }
    }
}