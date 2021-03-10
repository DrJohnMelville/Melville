

using Microsoft.Extensions.DependencyInjection;

namespace Melville.IOC.AspNet
{
    public static class BindMultipleInterfaces
    {
        public static void ForwardSingleton<TSource, TDest>(this IServiceCollection service)
            where TDest : TSource where TSource: class =>
            service.AddSingleton<TSource>(isp => isp.GetRequiredService<TDest>());
        public static void ForwardScoped<TSource, TDest>(this IServiceCollection service)
            where TDest : TSource where TSource: class => 
            service.AddScoped<TSource>(isp => isp.GetRequiredService<TDest>());
        public static void ForwardTransient<TSource, TDest>(this IServiceCollection service)
            where TDest : TSource where TSource: class =>
            service.AddScoped<TSource>(isp => isp.GetRequiredService<TDest>());
    }
}