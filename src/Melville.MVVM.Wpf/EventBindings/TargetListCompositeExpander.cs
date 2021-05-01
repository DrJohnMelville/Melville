using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Melville.MVVM.Wpf.EventBindings
{
    public sealed class TargetListCompositeExpander: CompositeListExpander
    {
        
        public TargetListCompositeExpander(IEnumerable<ITargetListExpander> expanders) :
            base(defaultExpanders.Concat(expanders))
        {
        }
        
        private static readonly ITargetListExpander[] defaultExpanders = {
            new ListExpander<FrameworkElement>(ExpandDataContext),
            new ListExpander<IAdditionlTargets>(ExpandAdditionalTargets)
        };

        private static void ExpandDataContext(FrameworkElement value, Action<object> target)
        {
            if (value.DataContext is {} dc) target(dc);
        }

        private static void ExpandAdditionalTargets(IAdditionlTargets value, Action<object> target)
        {
            foreach (var oneValue in value.Targets())
            {
                target(oneValue);
            }
        }
    }
}