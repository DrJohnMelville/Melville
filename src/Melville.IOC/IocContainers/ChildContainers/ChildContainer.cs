using System.Collections.Concurrent;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers.ChildContainers
{
    public class ChildContainer: IocContainer
    {
        public ChildContainer(IBindableIocService parent): base(CreateChildResolutionPolicy(parent))
        {
        }

        private static ITypeResolutionPolicyList CreateChildResolutionPolicy(IBindableIocService parent)
        {
            var paretCache = parent.ConfigurePolicy<CachedResolutionPolicy>();
            var template = new StandardTypeResolutionPolicy();
            template.AddResolutionPolicyAfter<CachedResolutionPolicy>(paretCache);
            return template;
        }
    }
}