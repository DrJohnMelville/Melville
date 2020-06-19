using System;
using System.Windows.Data;
using Melville.IOC.IocContainers;
using Melville.Log.NamedPipeEventSink;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.Wpf.Samples.SampleTreeViewDisplays;
using Melville.Wpf.Samples.ScopedMethodCalls;
using Melville.WpfAppFramework.StartupBases;
using Serilog;

namespace Melville.Wpf.Samples.ApplicationRoot
{
    public sealed class Startup:StartupBase
    {
        [STAThread]
        public static int Main(string[] commandLineArgs)
        {
            ApplicationRootImplementation.Run(new Startup());
            return 0;
        }

        protected override void RegisterWithIocContainer(IBindableIocService service)
        {
            service.AddLogging();
            service.RegisterHomeViewModel<SamplesTreeViewModel>();
            service.Bind<DisposableDependency>().ToSelf().AsScoped();
        }
    }
}