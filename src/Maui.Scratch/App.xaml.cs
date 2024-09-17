namespace Maui.Scratch;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        // Implementation plan for WaitingService.
        // I am going top create a child of Shell that AppShell can descend from.
        // This will expose through tree searching an operation that will push a modal waiting dialog on thw top of the nagivation stack.
        // the push operation will return an interface to control the wait dialog.  Disposing the controller will pop the wait dialog off of the
        // navigation stack.
    }
}
