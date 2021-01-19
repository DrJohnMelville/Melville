using System;
using Melville.IOC.IocContainers;
using Melville.WpfAppFramework.StartupBases;

namespace Melville.UdpConsoleViewer
{
    public class Startup : StartupBase
    {
        [STAThread]
        public static int Main(string[] args)
        {
            ApplicationRootImplementation.Run(new Startup(args));
            return 0;
        }

        public Startup(string[] commandLineParameters) : base(commandLineParameters)
        {
        }

        protected override void RegisterWithIocContainer(IBindableIocService service)
        {
            service.RegisterHomeViewModel<HomeViewModel>();

        }
    }
}