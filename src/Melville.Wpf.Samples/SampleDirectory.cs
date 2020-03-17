using System.Diagnostics.Tracing;
using Melville.Wpf.Samples.ApplicationBinding;
using Melville.Wpf.Samples.SampleTreeViewDisplays;

namespace Melville.Wpf.Samples
{
    public class SampleDirectory: SampleDirectoryDsl
    {
        public ISampleTreeItem DefaultItem() =>
            SearchTreeForSample<CallMethodOnApplicationViewModel>();
        
        public SampleDirectory()
        {
            Items.AddRange(new []
            {
                AppllicationIntegrationNode()
            });
        }

        private ISampleTreeItem AppllicationIntegrationNode() =>
            Node("Application Bindings",
                Page<CallMethodOnApplicationViewModel>("Call Method On Application")     
            );

    }
}