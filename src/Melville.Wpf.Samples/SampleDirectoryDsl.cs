using System.Collections.Generic;
using System.Linq;
using Melville.Linq;
using Melville.Wpf.Samples.SampleTreeViewDisplays;

namespace Melville.Wpf.Samples
{
    public abstract class SampleDirectoryDsl
    {
        // these methods for a mini-dsl for creating the tree in the sub-class
        public List<ISampleTreeItem> Items { get; } = new List<ISampleTreeItem>();
        protected ISampleTreeItem Node(string name, params ISampleTreeItem[] children) => 
            new SampleTreeNode(name, children);
        protected ISampleTreeItem Page<T>(string name) => new SampleTreeItemViewModel<T>(name);
        protected ISampleTreeItem? SearchTreeForSample<T>() =>
            Items.SelectRecursive(i => i.Children)
                .OfType<SampleTreeItemViewModel<T>>()
                .FirstOrDefault();
        
    }
}