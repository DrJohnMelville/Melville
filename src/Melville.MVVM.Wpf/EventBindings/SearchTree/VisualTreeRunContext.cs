using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.EventBindings.SearchTree;

public class VisualTreeRunContext: EventArgs
{
    public IDIIntegration DIIntegration { get; }
    public DependencyObject Root { get; }
    public string TargetMethodName { get; }
    public object?[] InputParameters { get; }
    public IList<object> Targets { get; }
    public IList<object> CandidateParameters { get; }

    public VisualTreeRunContext(
        IDIIntegration diIntegration, DependencyObject root, string targetMethodName,
        IEnumerable<object?> inputParameters)
    {
         DIIntegration = diIntegration;
        Root = root;
        TargetMethodName = targetMethodName;
        InputParameters = inputParameters.ToArray();
        Targets = TargetSelector.ResolveTarget(
            Root, diIntegration.GetRequired<TargetListCompositeExpander>());
        CandidateParameters =
            DIIntegration.GetRequired<ParameterListCompositeExpander>()
                .Expand(InputParameters.Concat(Targets));
    }
    public IEnumerable<object> AllParameterPossibilities() => CandidateParameters;
}