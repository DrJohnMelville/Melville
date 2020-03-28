using System.Diagnostics.Tracing;
using System.Windows;
using Melville.Wpf.Samples.ApplicationBinding;
using Melville.Wpf.Samples.LinqPadGraph;
using Melville.Wpf.Samples.SampleTreeViewDisplays;
using Melville.Wpf.Samples.ScopedMethodCalls;

namespace Melville.Wpf.Samples
{
    public class SampleDirectory: SampleDirectoryDsl
    {
        public ISampleTreeItem DefaultItem() =>
            SearchTreeForSample<ScopedMethodCallViewModel>();
        
        public SampleDirectory()
        {
            Items.AddRange(new []
            {
                AppllicationIntegrationNode()
            });
        }

        private ISampleTreeItem AppllicationIntegrationNode() =>
            Node("Root",
            Node("Event Binding",
                Page<CallMethodOnApplicationViewModel>("Call Method On Application"),
                Page<ScopedMethodCallViewModel>("Call Methods in a scope")
            ),
            Node("Statistics",
                Page<LinqGraphViewModel>("LinqPad Graphics Module"))
        );

    }
}