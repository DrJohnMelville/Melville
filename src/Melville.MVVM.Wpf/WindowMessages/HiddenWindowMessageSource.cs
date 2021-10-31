using System;
using System.Windows;

namespace Melville.MVVM.Wpf.WindowMessages;

public sealed class HiddenWindowMessageSource : WindowMessageSource, IDisposable
{
    public HiddenWindowMessageSource() : base(CreateWindow())
    {
    }

    private static Window CreateWindow()
    {
        var win = new Window()
        {
            Width = 0,
            Height = 0,
            WindowStyle = WindowStyle.None,
            ShowActivated = false,
            ShowInTaskbar = false
        };
        win.Show();
        win.Hide();
        return win;
    }

    public void Dispose()
    {
        SourceWindow.Close();
    }
}