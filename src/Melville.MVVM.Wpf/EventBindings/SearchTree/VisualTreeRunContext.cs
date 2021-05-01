using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.EventBindings.SearchTree
{
    public readonly struct VisualTreeRunContext
    {
        public IDIIntegration DIIntegration { get; }
        public DependencyObject Root { get; }
        public string TargetMethodName { get; }
        public object?[] InputParameters { get; }

        public VisualTreeRunContext(
            IDIIntegration diIntegration, DependencyObject root, string targetMethodName, 
            IEnumerable<object?> inputParameters)
        {
            DIIntegration = diIntegration;
            Root = root;
            TargetMethodName = targetMethodName;
            InputParameters = inputParameters.ToArray();
        }

        public IEnumerable<object> AllParameterPossibilities() =>
            InputParameters
                .Concat(Root.AllSources())
                .OfType<object>()
                .Distinct();
    }
}