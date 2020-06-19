using System.Collections.Generic;
using Melville.MVVM.BusinessObjects;
using Melville.WpfAppFramework.StartupBases;

namespace Melville.Wpf.Samples.SampleTreeViewDisplays
{
    public class SamplesTreeViewModel: NotifyBase
    {
        private ISampleTreeItem? currentItem;
        public ISampleTreeItem? CurrentItem 
        {
            get => currentItem;
            set => AssignAndNotify(ref currentItem, value);
        }
        
        public List<ISampleTreeItem> AllSamples { get; }

        public SamplesTreeViewModel(SampleDirectory directory)
        {
            AllSamples = directory.Items;
            CurrentItem = directory.DefaultItem();
        }
    }
}