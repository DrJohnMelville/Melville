using System.Net;
using System.Net.Http;
using Melville.IOC.IocContainers;

namespace Melville.WpfAppFramework.HttpsServices
{
    public static class HttpsServiceRegistration
    {
        public static void RegisterHttpClientWithSingleSource(this IBindableIocService ioc)
        {
            ioc.Bind<HttpMessageHandler>().To<HttpClientHandler>()
                .DoNotDispose()
                .WhenConstructingType<DoNotDisposeHttpMessageHandler>();
            ioc.Bind<HttpMessageHandler>().To<DoNotDisposeHttpMessageHandler>()
                .AsSingleton()
                .DisposeIfInsideScope()
                .BlockSelfInjection();
            ioc.Bind<HttpClient>().ToSelf().WithParameters(false);
        }
    }

    public class DoNotDisposeHttpMessageHandler : DelegatingHandler
    {
        public DoNotDisposeHttpMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override void Dispose(bool disposing)
        {
            // prevent the inner handler from being disposed
        }
    }
}