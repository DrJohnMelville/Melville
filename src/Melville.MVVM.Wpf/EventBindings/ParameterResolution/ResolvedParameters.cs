using System;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.MVVM.Wpf.EventBindings.ParameterResolution;

public readonly struct ResolvedParameters
{
    private readonly IFactory[] factories;

    public ResolvedParameters(in int numberOfParameters)
    {
        factories = new IFactory[numberOfParameters];
    }

    public void Put(int position, IFactory parameter)
    {
        factories[position] = parameter;
    }
    
    public IDisposable GetValues(ref VisualTreeRunContext context, out object?[] values)
    {
        var scope = context.DIIntegration.CreateScope();
        values = new object?[factories.Length];
        for (int i = 0; i < factories.Length; i++)
        {
            values[i] = factories[i].Create(scope, context.Root, context.InputParameters);
        }
        return scope;
    }
}