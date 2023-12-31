using System;
using Melville.INPC;
using Melville.IOC.IocContainers;
using Melville.WpfAppFramework.StartupBases;

namespace LocalNugetMonitor.Startup;

[FromConstructor]
public partial class Startup: StartupBase
{
    private Startup(string[] args) : base(args)
    {
    }

    [STAThread]
    public static int Main(string[] args)
    {
        ApplicationRootImplementation.Run(new Startup(args));
        return 0;
    }

    protected override void RegisterWithIocContainer(IBindableIocService service)
    {
        service.Bind<string>().ToConstant("Hello World");
        service.RegisterHomeViewModel<string>();

    }
}