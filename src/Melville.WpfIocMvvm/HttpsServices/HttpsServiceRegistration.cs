using System.Net;
using System.Net.Http;
using Melville.IOC.IocContainers;

namespace Melville.WpfIocMvvm.HttpsServices
{
    public static class HttpsServiceRegistration
    {
        public static void RegisterHttpClientWithSingleSource(this IBindableIocService ioc)
        {
            ioc.Bind<HttpMessageHandler>().To<HttpClientHandler>().AsSingleton().DisposeIfInsideScope();
            ioc.Bind<HttpClient>().ToSelf().WithParameters(false);
        }
    }
}