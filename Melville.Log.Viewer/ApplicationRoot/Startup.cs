using System.Drawing.Design;
using System.Windows.Data;
using Melville.IOC.IocContainers;
using Melville.Log.Viewer.NamedPipeServers;
using Melville.MVVM.AdvancedLists;

namespace Melville.Log.Viewer.ApplicationRoot
{
    public class Startup
    {
        private IocContainer ioc = new IocContainer();
        public  IIocService CompositionRoot() => ioc;

        public Startup()
        {
            UiThreadBuilder.SetFixupHook(BindingOperations.EnableCollectionSynchronization);

            SetupPipeListener();
        }

        private void SetupPipeListener()
        {
            ioc.Bind<IShutdownMonitor>().To<ShutdownMonitor>().AsSingleton();
            ioc.Bind<IPipeListener>().And<PipeListener>().To<PipeListener>().AsSingleton();
        }
    }
}