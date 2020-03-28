using System;
using System.Collections.Generic;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.TypeResolutionPolicy
{
    public sealed class GetIocServiceTypes: ITypeResolutionPolicy
    {
        public List<Type> Types { get; } = new List<Type>()
        {
            typeof(IIocService),
            typeof(IDisposableIocService),
            typeof(IocContainer),
            typeof(IBindableIocService)
        };
        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) =>
            Types.Contains(request.DesiredType) ?
                new MethodActivationStrategy<object>((service, bindingRequest)=>service): null;
    }
}