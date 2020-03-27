using Castle.DynamicProxy;
using Melville.IOC.IocContainers;

namespace Melville.Ioc.Interception.InterfaceFactories
{
    public class FactoryInterceptor: IInterceptor
    {
        private readonly IBindingRequest factoryRequest;
        public FactoryInterceptor(IBindingRequest factoryRequest)
        {
            this.factoryRequest = factoryRequest;
        }
        public void Intercept(IInvocation invocation)
        {
            var req = factoryRequest.CreateSubRequest(invocation.Method.ReturnType, invocation.Arguments);
            invocation.ReturnValue = factoryRequest.IocService.Get(req);
        }
    }
}