using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Melville.Lists;
using Melville.SystemInterface.USB;

namespace Melville.Wpf.Samples.HIDExplore
{
    public class HIDMonitorViewModel
    {
        public ThreadSafeBindableCollection<IList<ByteRecord>> Messages { get; } = 
            new ThreadSafeBindableCollection<IList<ByteRecord>>();
        private IMonitorForDeviceArrival newDeviceArrival;
        private readonly USBLogger logger;
        public HIDMonitorViewModel(string devicePath, IMonitorForDeviceArrival newDeviceArrival)
        {
            this.newDeviceArrival = newDeviceArrival;
            logger = new USBLogger(devicePath, newDeviceArrival, Messages);
        }

        private class USBLogger : UsbDevice
        {
            private readonly IList<IList<ByteRecord>> log;

            public USBLogger(string deviceId, IMonitorForDeviceArrival newDeviceNodification,
                IList<IList<ByteRecord>> log) :
                base(deviceId, newDeviceNodification)
            {
                this.log = log;
            }

            protected override void DeviceInputEvent(byte[] data)
            {
                log.Insert(0, MakeRecord(data));
            }

            private IList<ByteRecord> MakeRecord(byte[] data)
            {
                if (log.Count == 0)
                {
                    return data.Select(i => new ByteRecord(i.ToString("XX"), Brushes.Blue)).ToList();
                }
                else
                {
                    return log[0].Zip(data, (old, cur) => new ByteRecord(cur.ToString("X").PadLeft(2,'0'), old)).ToList();
                }
            }
        }
    }

    public class ByteRecord
    {
        public string Data { get; }
        public Brush Color { get; }

        public ByteRecord(string data, ByteRecord other) :
            this(data, data == other.Data ? Brushes.Black : Brushes.Red)
        {
        }

        public ByteRecord(string data, Brush color)
        {
            Data = data;
            Color = color;
        }
    }
}