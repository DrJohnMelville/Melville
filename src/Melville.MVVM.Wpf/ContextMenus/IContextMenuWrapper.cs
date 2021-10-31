using System;
using System.Windows.Controls;

namespace Melville.MVVM.Wpf.ContextMenus;

public interface IContextMenuWrapper
{
    void ClearMenu();
    void ClearAfter(int itemsToKeep);
    IContextMenuWrapper AddItem(object content, Action action, Action<MenuItem>? initialize = null);
    IContextMenuWrapper AddSubMenu(object content, Action<IContextMenuWrapper> initialize);
    IContextMenuWrapper AddItem(MenuItem item);
    IContextMenuWrapper AddSeparator();
    void CancelOpeningMenu();
}