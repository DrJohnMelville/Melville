using System.Net.Http;
using Melville.INPC;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;

namespace Melville.WpfAppFramework.HttpsServices;

public static class HttpsServiceRegistration
{
    public static void RegisterHttpClientWithSingleSource(this IBindableIocService ioc)
    {
        var handler = new DoNotDisposeHttpMessageHandler(
            new HttpClientHandler());
        ioc.Bind<HttpClient>().ToMethod(()=>new DoNotDisposeHttpClient(handler))
            .DisposeIfInsideScope();
    }
}

public class DoNotDisposeHttpMessageHandler(HttpMessageHandler innerHandler) : DelegatingHandler(innerHandler)
{
    protected override void Dispose(bool disposing)
    {
        // prevent the inner handler from being disposed
    }
}

public partial class DoNotDisposeHttpClient(HttpMessageHandler handler) : HttpClient(handler, false)
{
    protected override void Dispose(bool disposing)
    {
        // prevent the inner handler from being disposed
    }
}
