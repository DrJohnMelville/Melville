using System;
using Melville.MVVM.WindowMessages;

namespace Melville.MVVM.USB
{
    public interface IMonitorForDeviceArrival
    {
        event EventHandler<WindowMessageEventArgs> DeviceArrived;
    }
    public sealed class MonitorForDeviceArrival : IMonitorForDeviceArrival,  IDisposable
    {
        private IWindowMessageSource source;
        const int wmDeviceChanged = 537;
        const int deviceNodesChanged = 7;

        public MonitorForDeviceArrival(IWindowMessageSource source)
        {
            this.source = source;
            source.RegisterForMessage(wmDeviceChanged).MessageReceived += CheckMessage;
        }

        private void CheckMessage(object? sender, WindowMessageEventArgs e)
        {
            if ((int) e.WParam == deviceNodesChanged)
            {
                DeviceArrived?.Invoke(sender, e);
            }
        }
        public event EventHandler<WindowMessageEventArgs>? DeviceArrived;
        public void Dispose() => source.RegisterForMessage(wmDeviceChanged).MessageReceived -= CheckMessage;
    }
}