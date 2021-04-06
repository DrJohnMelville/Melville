using System.Net.Http;
using Melville.Hacks.Reflection;
using Melville.IOC.IocContainers;
using Melville.WpfAppFramework.HttpsServices;
using Xunit;

namespace Melville.MVVM.WPF.Test.WpfAppFRamework
{
    public class HttpRegistrationTest
    {
        [Fact]
        public void HttpClients()
        {
            var ioc = new IocContainer();
            ioc.RegisterHttpClientWithSingleSource();

            var c1 = ioc.Get<HttpClient>();
            Assert.False((c1.GetField("_disposeHandler") as bool?) ?? true);
        }

    }
}