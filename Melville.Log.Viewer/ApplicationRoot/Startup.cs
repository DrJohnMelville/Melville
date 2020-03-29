using System.Windows.Data;
using Melville.IOC.IocContainers;
using Melville.MVVM.AdvancedLists;

namespace Melville.Log.Viewer.ApplicationRoot
{
    public class Startup
    {
        private IocContainer ioc = new IocContainer();
        public  IIocService CompositionRoot() => ioc;

        public Startup()
        {
            ThreadSafeCollectionBuilder.SetFixupHook(BindingOperations.EnableCollectionSynchronization);
        }
    }
}