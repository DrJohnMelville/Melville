using Melville.Wpf.Samples.ApplicationBinding;
using Melville.Wpf.Samples.ApplicationBinding.ClipboardMonitor;
using Melville.Wpf.Samples.CalendarControls;
using Melville.Wpf.Samples.DialogBox;
using Melville.Wpf.Samples.DiBinding;
using Melville.Wpf.Samples.HIDExplore;
using Melville.Wpf.Samples.LinqPadGraph;
using Melville.Wpf.Samples.MouseClicks;
using Melville.Wpf.Samples.SampleTreeViewDisplays;
using Melville.Wpf.Samples.ScopedMethodCalls;
using Melville.Wpf.Samples.ShellCommands;
using Melville.Wpf.Samples.ThumbDrives;
using Melville.Wpf.Samples.TranscriptionPedal;
using Melville.Wpf.Samples.TreeDragging;
using Melville.Wpf.Samples.WaitMessageTest;
using Melville.Wpf.Samples.WebView2Integration;

namespace Melville.Wpf.Samples;

public class SampleDirectory : SampleDirectoryDsl
{
    public ISampleTreeItem? DefaultItem() =>
        SearchTreeForSample<DialogBoxSheetViewModel>();

    public SampleDirectory()
    {
        Items.AddRange(new[]
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
                Page<ChYokeViewModel>("CH Yoke"),
                Page<ThumbDriveViewModel>("Detect Thumb Drive Arrival"),
                Page<ClipboardMonitorViewModel>("Clipboard Monitor"),
                Page<HidDeviceEnumerationViewModel>("ListHIDDevice"),
                Page<DefaultBrowserViewModel>("Launch System Browser")
            ),
            Node("Event Binding",
                Page<CallMethodOnApplicationViewModel>("Call Method On Application"),
                Page<ScopedMethodCallViewModel>("Call Methods in a scope"),
                Page<WebView2ViewModel>("WebView2 Events"),
                Page<DiBindingViewModel>("DI Binding"),
                Page<WaitSampleViewModel>("Waiting Screen"),
                Page<MouseClickViewModel>("Mouse Clicks"),
                Page<TreeDraggingViewModel>("Tree Dragging")
            ),
            Node("Statistics",
                Page<LinqGraphViewModel>("LinqPad Graphics Module")),
            Node("Controls",
                Page<CalendarControlViewModel>("Calendar Control"),
                Page<DialogBoxSheetViewModel>("Dialog Box"))
        );
}