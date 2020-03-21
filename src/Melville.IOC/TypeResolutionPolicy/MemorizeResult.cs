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


        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request)
        {
            var ret = InnerPolicy.ApplyResolutionPolicy(request);
            if (ret != null)
            {
                cache.Bind(request.DesiredType, true).DoBinding(ret);
            }

            return ret;
        }
    }
}