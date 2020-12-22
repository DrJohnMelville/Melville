using System;

namespace Melville.MVVM.USB.Joysticks
{
    public abstract class JoystickBase : UsbDevice
    {
        private byte[] buffer;

        protected JoystickBase(string deviceId, IMonitorForDeviceArrival newDeviceNodification, int dataLen) 
            : base(deviceId, newDeviceNodification)
        {
            buffer = new byte[dataLen];
        }
        protected override void DeviceInputEvent(byte[] data)
        {
            buffer = data;
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs>? StateChanged;
        protected byte GetAxis(int position) => buffer[position];
        protected int Get16BitAxis(int position) => GetAxis(position) | (GetAxis(position + 1) << 8);
        protected bool GetButton(int position, byte mask) => (buffer[position] & mask) == mask;
    }
}