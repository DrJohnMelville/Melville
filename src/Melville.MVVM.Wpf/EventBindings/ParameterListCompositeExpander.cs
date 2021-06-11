using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Melville.MVVM.Wpf.ContextMenus;
using Melville.MVVM.Wpf.KeyboardFacade;
using Melville.MVVM.Wpf.MouseClicks;

namespace Melville.MVVM.Wpf.EventBindings
{
    public sealed class ParameterListCompositeExpander: CompositeListExpander
    {
        private static IParameterListExpander[] builtInExpanders =
        {
            new MouseClickWrapper(),
            new KeyEventWrapper(),
            new ListExpander<ContextMenuEventArgs>((e,target)=>target.Invoke(new ContextMenuWrapper(e)))
        };
        public ParameterListCompositeExpander(IEnumerable<IParameterListExpander> expanders) : base(
            builtInExpanders.Concat(expanders))
        {
        }
    }
}