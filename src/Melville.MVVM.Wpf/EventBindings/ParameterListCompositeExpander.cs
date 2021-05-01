using System.Collections.Generic;
using System.Linq;
using Melville.MVVM.Wpf.MouseClicks;

namespace Melville.MVVM.Wpf.EventBindings
{
    public sealed class ParameterListCompositeExpander: CompositeListExpander
    {
        private static IParameterListExpander[] builtInExpanders =
        {
            new MouseClickWrapper()
        };
        public ParameterListCompositeExpander(IEnumerable<IParameterListExpander> expanders) : base(
            builtInExpanders.Concat(expanders))
        {
        }
    }
}