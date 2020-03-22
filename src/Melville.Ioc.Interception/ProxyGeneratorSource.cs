using System;
using Castle.DynamicProxy;
using Melville.IOC.IocContainers;

namespace Melville.Ioc.Interception
{
    public static class ProxyGeneratorSource
    {
        private static volatile ProxyGenerator? source;
        private static object mutex = new object();

        public static ProxyGenerator GetSource(IIocService service)
        {
            if (source == null)
            {
                lock (mutex)
                {
                    if (source == null)
                    {
                        source = service.Get<ProxyGenerator>();
                    }
                }
            }

            return source;
        }

        public static object CreateInterceptor(IIocService service, Type type, object target,
            IInterceptor[] interceptors) =>
            GetSource(service).CreateInterfaceProxyWithTarget(type, target, interceptors);
    }
}