using System;
using System.Collections;
using System.Windows.Controls;
using Melville.INPC;

namespace Melville.MVVM.Wpf.ContextMenus;

public partial class SubMenuWrapper : IContextMenuWrapper
{
    [FromConstructor] private readonly IList menu;
    
    public void ClearMenu() => menu.Clear();

    public void ClearAfter(int itemsToKeep)
    {
        for (int i = menu.Count-1; i >= itemsToKeep; i--)
        {
            menu.RemoveAt(i);
        }
    }

    public IContextMenuWrapper AddItem(object content, Action action, Action<MenuItem>? initialize)
    {
        var mi = new MenuItem
        {
            Header = content,
        };
        mi.Click += (_, _) => action();
        initialize?.Invoke(mi);
        return AddItem(mi);
    }

    public IContextMenuWrapper AddItem(MenuItem content)
    {
        menu.Add(content);
        return this;
    }

    public IContextMenuWrapper AddSubMenu(object content, Action<IContextMenuWrapper> initialize)
    {
        var mi = new MenuItem()
        {
            Header = content
        };
        initialize(new SubMenuWrapper(mi.Items));
        return AddItem(mi);
    }

    public IContextMenuWrapper AddSeparator()
    {
        menu.Add(new Separator());
        return this;
    }

    public virtual void CancelOpeningMenu() { }
}