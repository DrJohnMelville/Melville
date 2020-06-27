using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Melville.MVVM.Wpf.USB
{
    public static class WindowHookMessages
    {
        public static void AttachWindowHook(this Visual vis, int msg, Action<IntPtr, IntPtr> method) =>
            (PresentationSource.FromVisual(vis) as HwndSource)?.AddHook(new HWndRegistration(msg, (a,b)=>
            {
                method(a,b);
                return false;
            }).Method);

        private class HWndRegistration
        {
            private int message;
            private Func<IntPtr, IntPtr, bool> method;

            public HWndRegistration(int message, Func<IntPtr, IntPtr, bool> method)
            {
                this.message = message;
                this.method = method;
            }

            public IntPtr Method(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
            {
                handled = msg==message && method(wparam, lparam);
                return IntPtr.Zero;
            }
        }
    }
}