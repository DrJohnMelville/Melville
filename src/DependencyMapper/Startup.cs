using System.Dynamic;
using System.Windows;
using DependencyMapper.Views;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.MvvmDialogs;
using Melville.MVVM.Wpf.RootWindows;
using Melville.WpfAppFramework.StartupBases;

namespace DependencyMapper;

public class Startup: StartupBase
{
    [STAThread]
    public static void Main()
    {
        ApplicationRootImplementation.Run(new Startup());
    }

    protected override void RegisterWithIocContainer(IBindableIocService service)
    {
        service.Bind<INavigationWindow>().To<NavigationWindow>().AsSingleton();
        service.Bind<RootNavigationWindow>().And<Window>().And<IRootNavigationWindow>()
            .ToSelf().FixResult(SetIcon).AsSingleton();
        service.Bind<IOpenSaveFile>().To<OpenSaveFileAdapter>();

        service.RegisterHomeViewModel<MainViewModel>();
    }

    private void SetIcon(RootNavigationWindow arg) => 
        arg.SetWindowIconFromResource("DependencyMapper", "app.ico");

}