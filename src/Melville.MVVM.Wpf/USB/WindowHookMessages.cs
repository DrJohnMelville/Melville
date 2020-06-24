using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Melville.MVVM.USB;

namespace Melville.MVVM.Wpf.USB
{
    public static class UsbRegistration
    {
        const int wmDeviceChange1d = 537;
        const int deviceNodesChanged = 7;
        public static void MonitorForDeviceArrival(this UsbDevice device, Window messageTarget)
        {
            messageTarget.AttachWindowHook(wmDeviceChange1d, (wparam, lparam) =>
            {
                if (((int) wparam) == deviceNodesChanged) device.TryConnect();
            });
            device.TryConnect();  // but also check if it is already here.
        }

        private static bool IsUsbDeviceChangedMessage(int msg, IntPtr wparam)
        {
            return msg == wmDeviceChange1d && ((int) wparam) == deviceNodesChanged;
        }
    }
    public static class WindowHookMessages
    {
        public static void AttachWindowHook(this Visual vis, HwndSourceHook hook) =>
            (PresentationSource.FromVisual(vis) as HwndSource)?.AddHook(hook);
        public static void AttachWindowHook(this Visual vis, int msg, Func<IntPtr, IntPtr, bool> method) =>
            (PresentationSource.FromVisual(vis) as HwndSource)?.AddHook(new HWndRegistration(msg, method).Method);
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