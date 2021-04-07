using System;
using System.Windows;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.RootWindows;
using Melville.MVVM.Wpf.WindowMessages;
using Melville.SystemInterface.USB;
using Melville.SystemInterface.USB.Pedal;
using Melville.SystemInterface.USB.ThumbDrives;
using Melville.SystemInterface.WindowMessages;
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
            service.Bind<SamplesTreeViewModel>().ToSelf().AsSingleton();
            service.RegisterHomeViewModel<SamplesTreeViewModel>();
            service.Bind<IRootNavigationWindow>()
                .And<Window>()
                .To<RootNavigationWindow>()
                .AsSingleton();
            service.Bind<DisposableDependency>().ToSelf().AsScoped();
            service.Bind<IWindowMessageSource>().To<WindowMessageSource>().AsSingleton();

            // pedal reader
            service.Bind<IMonitorForDeviceArrival>().To<MonitorForDeviceArrival>()
                .AsSingleton().DoNotDispose();
            service.Bind<ITranscriptonPedal>().To<TranscriptonPedal>()
                .DoNotDispose();
            service.Bind<IDetectThumbDrive>().To<DetectThumbDrive>().AsSingleton().DoNotDispose();
        }
    }
}