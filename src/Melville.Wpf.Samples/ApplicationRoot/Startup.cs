using System;
using System.Windows;
using Melville.IOC.IocContainers;
using Melville.MVVM.USB;
using Melville.MVVM.USB.Pedal;
using Melville.MVVM.Wpf.Clipboards;
using Melville.MVVM.Wpf.RootWindows;
using Melville.MVVM.Wpf.USB;
using Melville.Wpf.Samples.SampleTreeViewDisplays;
using Melville.Wpf.Samples.ScopedMethodCalls;
using Melville.WpfAppFramework.StartupBases;

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
            service.Bind<IRootNavigationWindow>()
                .And<Window>()
                .To<RootNavigationWindow>()
                .AsSingleton();
            service.Bind<DisposableDependency>().ToSelf().AsScoped();

            // pedal reader
            service.Bind<ITranscriptonPedal>().To<TranscriptonPedal>()
                .FixResult((i, request) => 
                    ((UsbDevice) i).MonitorForDeviceArrival(request.IocService.Get<Window>()))
                .AsSingleton()
                .DoNotDispose();
            
            // cliboard
            service.Bind<IClipboardNotification>().To<ClipboardNotification>().AsSingleton();
        }
    }
}