using System.Collections.Generic;
using System.Linq;
using Windows.Devices.PointOfService;
using Melville.Hacks;
using Melville.Lists;

namespace Melville.Wpf.Samples.TreeDragging;

public class TreeItem
{
    public string Name { get; }

    public TreeItem(string name)
    {
        Name = name;
    }
}

public class TreeNode : TreeItem
{
    public IList<TreeItem> Children { get; } = new ThreadSafeBindableCollection<TreeItem>();

    public TreeNode(string name, params TreeItem[] children) : base(name)
    {
        Children.AddRange(children);
    }
}
public class TreeDraggingViewModel
{
    public IList<TreeItem> Data { get; } = new ThreadSafeBindableCollection<TreeItem>()
    {
        new TreeNode("Mamals",
            new TreeItem("Dog"),
            new TreeItem("Cat"),
            new TreeItem("Sheep")
        ),
        new TreeNode("Insects",
            new TreeItem("LadyBug"),
            new TreeItem("Catipiller"),
            new TreeItem("Spider")
        ),
        new TreeNode("Plants",
            new TreeItem("Rose"),
            new TreeItem("California Redwood"),
            new TreeItem("Mary Jane the weed")
        )
    };
}