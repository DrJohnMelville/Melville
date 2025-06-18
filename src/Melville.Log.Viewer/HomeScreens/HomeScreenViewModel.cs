using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Melville.FileSystem;
using Melville.Lists;
using Melville.Log.Viewer.LogViews;
using Melville.Log.Viewer.NugetMonitor;
using Melville.Log.Viewer.UdpServers;
using Melville.Log.Viewer.WelcomePage;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;

namespace Melville.Log.Viewer.HomeScreens;

public interface IHomeScreenPage
{
    string Title { get; }
    void Stop();
}
public class HomeScreenViewModel: NotifyBase
{
    private IHomeScreenPage currentPage;
    public IHomeScreenPage CurrentPage
    {
        get => currentPage;
        set => AssignAndNotify(ref currentPage, value);
    }

    public ICollection<IHomeScreenPage> Pages { get; } = new ThreadSafeBindableCollection<IHomeScreenPage>();

    public HomeScreenViewModel(WelcomePageViewModel welcomePage, 
        Func<ILogConnection, LogViewModel> modelCreator)
    {
        Pages.Add(welcomePage);
        currentPage = welcomePage;
    }

    public void Remove(IHomeScreenPage page)
    {
        Pages.Remove(page);
        page.Stop();
    }

    public bool ShellKeyHandler(KeyEventArgs e)
    {
        if (e.Key == Key.F4 && e.KeyboardDevice.Modifiers == ModifierKeys.Control &&
            CurrentPage is LogViewModel)
        {
            Remove(CurrentPage);
            return true;
        }

        return false;
    }
    
    public void ConnectToUdp()
    {
        AddNewPage(new LogViewModel(new UdpLogConnection(), "Udp Logger"));
    }

    public void CopyUdpSender()
    {
        Clipboard.SetText("""
            public static class UdpConsole
            {
                private static UdpClient? client = null;
                private static UdpClient Client
                {
                    get
                    {
                        client ??= new UdpClient();
                        return client ;
                    }
                }
            
                public static string WriteLine(string str)
                {
                    var bytes = Encoding.UTF8.GetBytes(str);
                    Client.Send(bytes, bytes.Length, "127.0.0.1", 15321);
                    return str;
                }
            }
            """);
    }

    public void MonitorLocalNuget(
        [FromServices] LogConsole console,
        [FromServices] IOpenSaveFile osf,
        [FromServices] Func<IDirectory, ILogConsole, NugetServiceFolderMonitor> fact)
    {
        if (osf.GetDirectory() is not { } dir) return;
        AddNewPage(new LogViewModel(console, "Local Nuget Monitor"));
        fact(dir, console);
    }

    private void AddNewPage(LogViewModel page)
    {
        Pages.Add(page);
        CurrentPage = page;
    }
}