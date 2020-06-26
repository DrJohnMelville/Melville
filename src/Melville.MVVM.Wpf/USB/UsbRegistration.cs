using System;
using System.Windows;
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
}