using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers.ChildContainers
{
    public class ChildContainer: IocContainer
    {
        public ChildContainer(IBindableIocService parent)
        {
            var paretCache = parent.ConfigurePolicy<CachedResolutionPolicy>();
            ConfigurePolicy<ISetBackupCache>().SetBackupCache(paretCache);
        }

    }
}