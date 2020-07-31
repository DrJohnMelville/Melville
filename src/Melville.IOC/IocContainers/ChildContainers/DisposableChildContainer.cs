using System.Threading.Tasks;

namespace Melville.IOC.IocContainers.ChildContainers
{
    public class DisposableChildContainer : ChildContainer, IDisposableIocService, IRegisterDispose
    {
        private readonly DisposalRegister innerService = new DisposalRegister();
        public DisposableChildContainer(IBindableIocService parent) : base(parent)
        {
        }

        public ValueTask DisposeAsync() => innerService.DisposeAsync();
        public void Dispose() => innerService.Dispose();
        public void RegisterForDispose(object obj)
        {
            if (obj == this) return;
            innerService.RegisterForDispose(obj);
        }
    }
}