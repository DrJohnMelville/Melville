using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Interop;
using Melville.SystemInterface.WindowMessages;

namespace Melville.MVVM.Wpf.WindowMessages
{
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
    public class WindowMessageSource:IWindowMessageSource
    {
        public IntPtr SourceWindowHWnd { get; }
        protected Window SourceWindow { get; }
        public WindowMessageSource(Window win)
        {
            if (win == null) throw new ArgumentNullException("win");
            SourceWindow = win;
            if (!(PresentationSource.FromVisual(win) is HwndSource source))
            {
                throw new InvalidOperationException("Cound not create HwndSource.");
            }

            SourceWindowHWnd = source.Handle;
            source.AddHook(HookMethod);
        }

        private IntPtr HookMethod(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (!registrations.TryGetValue(msg, out var handler)) return IntPtr.Zero;

            var args = new WindowMessageEventArgs(hwnd, msg, wparam,lparam, handled);
            handler.Invoke(args);
            handled = args.Handled;
            return args.ReturnValue;
        }

        private ConcurrentDictionary<int, SingleWindowEventHolder> registrations = 
            new ConcurrentDictionary<int, SingleWindowEventHolder>();
        
        public ISingleWindowEventHolder RegisterForMessage(int message) =>
            registrations.GetOrAdd(message, i => new SingleWindowEventHolder());

        public class SingleWindowEventHolder: ISingleWindowEventHolder
        {
            public event EventHandler<WindowMessageEventArgs>? MessageReceived;
            public void Invoke(WindowMessageEventArgs args) => MessageReceived?.Invoke(this, args);
        }
    }
}