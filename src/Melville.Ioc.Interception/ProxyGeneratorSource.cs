using System;
using Castle.DynamicProxy;
using Melville.IOC.IocContainers;

namespace Melville.Ioc.Interception
{
    public static class ProxyGeneratorSource
    {
        private static volatile IProxyGenerator? source;
        private static object mutex = new object();

        public static IProxyGenerator GetSource(IIocService service)
        {
            if (source == null)
            {
                lock (mutex)
                {
                    if (source == null)
                    {
                        source = service.CanGet(typeof(IProxyGenerator))?
                            service.Get<IProxyGenerator>():
                            new ProxyGenerator();
                    }
                }
            }

            return source;
        }

        public static object CreateInterceptor(IIocService service, Type type, object target,
            params IInterceptor[] interceptors) =>
            GetSource(service).CreateInterfaceProxyWithTarget(type, target, interceptors);
        public static object CreateSyntheticObject(IIocService service, Type type, params IInterceptor[] interceptors) =>
            GetSource(service).CreateInterfaceProxyWithoutTarget(type, interceptors);
    }
}