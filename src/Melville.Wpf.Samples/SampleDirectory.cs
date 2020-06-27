using System.Diagnostics.Tracing;
using System.Windows;
using Melville.Wpf.Samples.ApplicationBinding;
using Melville.Wpf.Samples.ApplicationBinding.ClipboardMonitor;
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
            SearchTreeForSample<ThumbDriveViewModel>();
        
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
                Page<ThumbDriveViewModel>("Transcription Pedal"),            
                Page<ClipboardMonitorViewModel>("Clipboard Monitor")
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