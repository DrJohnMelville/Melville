using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Melville.Wpf.Samples.ApplicationBinding;
using Melville.Wpf.Samples.SampleTreeViewDisplays;

namespace Melville.Wpf.Samples
{
    public class SampleDirectory
    {
        public List<ISampleTreeItem> Items { get; } = new List<ISampleTreeItem>();

        private ISampleTreeItem Node(string name, params ISampleTreeItem[] children) => 
            new SampleTreeNode(name, children);
        private ISampleTreeItem Page<T>(string name) => new SampleTreeItemViewModel<T>(name);
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