using Melville.IOC.AspNet.RegisterFromServiceCollection;
using Melville.IOC.IocContainers;
using Melville.IOC.Test.IocContainers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Melville.IOC.Test.ServiceCollectionIntegration
{
    public class ServiceProviderFactory
    {
        private readonly IServiceProviderFactory<IocContainer> sut = new MelvilleServiceProviderFactory();

        [Fact]
        public void Integration()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton("Foo Bar");
            var builder = sut.CreateBuilder(sc);
            Assert.NotNull(builder);
            var provider = sut.CreateServiceProvider(builder);
            Assert.Equal("Foo Bar", provider.GetRequiredService<string>());
        }

        [Fact]
        public void Scoped()
        {
            var sc = new ServiceCollection();
            sc.AddScoped<ISimpleObject, SimpleObjectImplementation>();
            var prov = sut.CreateServiceProvider(sut.CreateBuilder(sc));
            using (var scope = prov.CreateScope())
            {
                Assert.NotNull(scope.ServiceProvider.GetService<ISimpleObject>());               
            }
        }
    }
}