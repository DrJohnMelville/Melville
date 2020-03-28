using Castle.DynamicProxy;

namespace Melville.Linq.Statistics.Graphics
{
    public static class MultiProxy
    {
        private static readonly ProxyGenerator pg = new ProxyGenerator();
        public static T Create<T>(params T[] targets) => 
            (T)pg.CreateInterfaceProxyWithoutTarget(typeof(T), new DoAllInterceptor<T>(targets));

        private class DoAllInterceptor<T> : IInterceptor
        {
            private readonly T[] targets;

            public DoAllInterceptor(T[] targets)
            {
                this.targets = targets;
            }

            public void Intercept(IInvocation invocation)
            {
                foreach (var target in targets)
                {
                    invocation.ReturnValue = invocation.Method.Invoke(target, invocation.Arguments);
                }
            }
        }
    }
}