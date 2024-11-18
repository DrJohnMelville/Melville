using Melville.MVVM.Maui.Commands;
using Melville.MVVM.Maui.WaitingService;

namespace Maui.Scratch;

public partial class App : WaitableApplication
{
    public App()
    {
        InitializeComponent();

        var mainPage = new AppShell();
        MainPage = mainPage;
    }
}