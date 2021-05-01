using System.Collections.Generic;

namespace Melville.MVVM.Wpf.EventBindings
{
    public sealed class ParameterListCompositeExpander: CompositeListExpander
    {
        public ParameterListCompositeExpander(IEnumerable<ITargetListExpander> expanders) : base(expanders)
        {
        }
    }
}