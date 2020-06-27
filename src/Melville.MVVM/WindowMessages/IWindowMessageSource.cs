using System;

namespace Melville.MVVM.WindowMessages
{
    public class WindowMessageEventArgs:EventArgs
    {
        public IntPtr Hwnd { get; }
        public int Message { get; }
        public IntPtr WParam { get; }
        public IntPtr LParam { get; }
        public bool Handled { get; set; }
        public IntPtr ReturnValue { get; set; } = IntPtr.Zero;

        public WindowMessageEventArgs(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, bool handled)
        {
            Hwnd = hwnd;
            Message = message;
            WParam = wParam;
            LParam = lParam;
            Handled = handled;
        }
    }

    public interface ISingleWindowEventHolder
    {
        event EventHandler<WindowMessageEventArgs>? MessageReceived;
    }
    public interface IWindowMessageSource
    {
        IntPtr SourceWindowHWnd { get; }
        ISingleWindowEventHolder RegisterForMessage(int message);
    }
}