using System;
using System.Windows;
using System.Windows.Controls;

namespace Melville.MVVM.Wpf.ContextMenus
{
    public class ContextMenuWrapper: SubMenuWrapper
    {
        private readonly ContextMenuEventArgs eventArgs;
        
        public ContextMenuWrapper(ContextMenuEventArgs eventArgs): base(
            CreateMenu(eventArgs))
        {
            this.eventArgs = eventArgs;
        }

        private static ItemCollection CreateMenu(ContextMenuEventArgs eventArgs) =>
            (eventArgs.Source is FrameworkElement fe)?
                GetOrCreateContextMenu(eventArgs,fe).Items :
                throw new InvalidOperationException("Must be a framework element");

        private static ContextMenu GetOrCreateContextMenu(ContextMenuEventArgs eventArgs, FrameworkElement fe) => 
            fe.ContextMenu ?? CreateContextMenu(eventArgs, fe);

        private static ContextMenu CreateContextMenu(ContextMenuEventArgs eventArgs, FrameworkElement fe)
        {
            var cm = new ContextMenu();
            fe.ContextMenu = cm;
            CancelMenuOpenAndRetrigger(eventArgs, cm);
            return cm;
        }

        private static void CancelMenuOpenAndRetrigger(ContextMenuEventArgs eventArgs, ContextMenu cm)
        {
            eventArgs.Handled = true;
            cm.IsOpen = true;
        }

        public override void CancelOpeningMenu()
        {
            eventArgs.Handled = true;
        }
    }

}