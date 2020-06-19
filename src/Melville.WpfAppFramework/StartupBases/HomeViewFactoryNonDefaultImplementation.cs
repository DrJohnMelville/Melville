using System;
using Melville.IOC.IocContainers;

namespace Melville.WpfAppFramework.StartupBases
{
    public interface IHomeViewModelFactory
    {
        object? Create(IIocService ioc);
    }

    public class HomeViewModelFactoryNonDefaultImplementation : IHomeViewModelFactory
    {
        private Func<IIocService, object?> creator;

        public HomeViewModelFactoryNonDefaultImplementation(Func<IIocService, object?> creator)
        {
            this.creator = creator;
        }

        public object? Create(IIocService ioc) => creator(ioc);
    }
}