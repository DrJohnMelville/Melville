using System.Collections;
using System.Collections.Generic;
using Melville.IOC.IocContainers;
using Melville.Log.Viewer.HomeScreens;
using Melville.Log.Viewer.NamedPipeServers;
using Microsoft.Extensions.Configuration;

namespace Melville.Log.Viewer.ApplicationRoot
{
    public class Startup
    {
        private IocContainer ioc = new IocContainer();
        public  IIocService CompositionRoot() => ioc;

        public Startup()
        {
            SetupConfiguration();
            SetupPipeListener();
        }

        private void SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .Build();

            ioc.Bind<IList<TargetSite>>()
                .ToMethod(i=>
                {
                    var ret= new List<TargetSite>();
                    builder.Bind("Links", ret);
                    return ret; 
                })
                .AsSingleton();
        }

        private void SetupPipeListener()
        {
            ioc.Bind<IShutdownMonitor>().To<ShutdownMonitor>().AsSingleton();
            ioc.Bind<IPipeListener>().And<PipeListener>().To<PipeListener>().AsSingleton();
        }
    }
}