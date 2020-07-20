using Melville.Wpf.Samples.ApplicationBinding;
using Melville.Wpf.Samples.ApplicationBinding.ClipboardMonitor;
using Melville.Wpf.Samples.HIDExplore;
using Melville.Wpf.Samples.LinqPadGraph;
using Melville.Wpf.Samples.SampleTreeViewDisplays;
using Melville.Wpf.Samples.ScopedMethodCalls;
using Melville.Wpf.Samples.ThumbDrives;
using Melville.Wpf.Samples.TranscriptionPedal;
using Melville.Wpf.Samples.WebView2Integration;

namespace Melville.Wpf.Samples
{
    public class SampleDirectory: SampleDirectoryDsl
    {
        public ISampleTreeItem DefaultItem() =>
            SearchTreeForSample<JoystickTranscriptionPedalViewModel>();
        
        public SampleDirectory()
        {
            Items.AddRange(new []
            {
                AppllicationIntegrationNode()
            });
        }

        private ISampleTreeItem AppllicationIntegrationNode() =>
            Node("Root",
            Node("Hardware",
                Page<TranscriptionPedalViewModel>("Transcription Pedal"),            
                Page<JoystickTranscriptionPedalViewModel>("Joystick emulating Transcription Pedal"),            
                Page<JoystickViewModel>("Joystick"),            
                Page<ThumbDriveViewModel>("Detect Thumb Drive Arrival"),            
                Page<ClipboardMonitorViewModel>("Clipboard Monitor"),
                Page<HidDeviceEnumerationViewModel>("ListHIDDevice")
            ),
            Node("Event Binding",
                Page<CallMethodOnApplicationViewModel>("Call Method On Application"),
                Page<ScopedMethodCallViewModel>("Call Methods in a scope"),
                Page<WebView2ViewModel>("WebView2 Events")
            ),
            Node("Statistics",
                Page<LinqGraphViewModel>("LinqPad Graphics Module"))
        );

    }
}