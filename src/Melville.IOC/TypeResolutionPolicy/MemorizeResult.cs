using System;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy
{
    public class MemorizeResult : ITypeResolutionPolicy
    {
        private CachedResolutionPolicy cache;
        public ITypeResolutionPolicy InnerPolicy { get; }

        public MemorizeResult(CachedResolutionPolicy cache, ITypeResolutionPolicy innerPolicy)
        {
            this.cache = cache;
            InnerPolicy = innerPolicy;
        }


        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) =>
            InnerPolicy.ApplyResolutionPolicy(request) switch
            {
                null => null,
                ObjectFactory obj => throw new InvalidProgramException("this should never happen."),
                var ret => cache.Bind(request.DesiredType, true).DoBinding(ret).GetFinalFactory()
            };
    }
}