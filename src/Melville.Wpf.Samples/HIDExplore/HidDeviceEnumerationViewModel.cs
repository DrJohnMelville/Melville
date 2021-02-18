using System;
using System.Collections.Generic;
using System.Linq;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.USB;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.ViewFrames;
using Melville.Wpf.Samples.SampleTreeViewDisplays;

namespace Melville.Wpf.Samples.HIDExplore
{
    [OnDisplayed(nameof(QueryDevices))]
    public class HidDeviceEnumerationViewModel: NotifyBase
    {
        public HidDeviceEnumerationViewModel(IMonitorForDeviceArrival arrival)
        {
            arrival.DeviceArrived += (s, e) => QueryDevices();
        }

        private List<string>? devices;
        public List<string>? Devices
        {
            get => devices;
            set => AssignAndNotify(ref devices, value);
        }

        private string selectedDevice = "";
        public string SelectedDevice
        {
            get => selectedDevice;
            set => AssignAndNotify(ref selectedDevice, value);
        }

        

        
        public void QueryDevices()
        {
            Devices = EnumDevices.Enum().ToList();
        }

        public void OpenDevice([FromServices] SamplesTreeViewModel rootWin,
            [FromServices] Func<string,HIDMonitorViewModel> viewerFactory)
        {
            rootWin.CurrentItem = new FakeSampleTreeItem(viewerFactory(SelectedDevice));
        }
        private class FakeSampleTreeItem: ISampleTreeItem
        {
            public string Title => "Fake Item";
            public object Content { get; }
            public IList<ISampleTreeItem> Children => Array.Empty<ISampleTreeItem>();

            public FakeSampleTreeItem(object content)
            {
                Content = content;
            }
        }
    }
}