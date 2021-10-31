using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.ViewFrames;

namespace Melville.Wpf.Samples.SampleTreeViewDisplays;

public interface ISampleTreeItem
{
    string Title { get; }
    object Content { get; }
    IList<ISampleTreeItem> Children { get; }
}

public class SampleTreeNode: ISampleTreeItem
{
    public string Title { get; }
    public object Content => Title;
    public IList<ISampleTreeItem> Children { get; }

    public SampleTreeNode(string title, IList<ISampleTreeItem> children)
    {
        Title = title;
        Children = children;
    }
}
public class SampleTreeItem: NotifyBase, ISampleTreeItem
{
    public string Title { get; }
    public object Content => this;
    public IList<ISampleTreeItem> Children => Array.Empty<ISampleTreeItem>();

    public SampleTreeItem(string title)
    {
        Title = title;
    }
}
[OnDisplayed("CreateItem")]
public class SampleTreeItemViewModel<T>: SampleTreeItem
{
    private object? item;
    public object? Item 
    {
        get => item;
        set => AssignAndNotify(ref item, value);
    }
    public SampleTreeItemViewModel(string title):base(title)
    {
    }

    public Task CreateItem([FromServices] T innerItem)
    {
        Item = innerItem;
        return Task.CompletedTask;
    }
}