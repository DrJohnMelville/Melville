using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;

public class MultiConstructorStrategy : IActivationStrategy
{
    private List<Lazy<IActivationStrategy>> constructors;
    public MultiConstructorStrategy(IList<ConstructorInfo> list)
    {
        constructors = list
            .Select(i => new Lazy<IActivationStrategy>(() => TypeActivatorFactory.StrategyFromConstructor(i)))
            .ToList();
    }

    public bool CanCreate(IBindingRequest bindingRequest) => 
        constructors.Any(i => i.Value.CanCreate(bindingRequest));

    public object? Create(IBindingRequest bindingRequest)
    {
        foreach (var constructor in constructors.Where(i=>i.Value.CanCreate(bindingRequest)))
        {
            try
            {
                return constructor.Value.Create(bindingRequest);
            }
            catch (IocException)
            {
                // try the next constructor until one works
            }
        }
        // if we get to here then we failed.  So we need to repeat the first try to let
        // the exception bubble up.
        return constructors[0].Value.Create(bindingRequest);
    }

    public SharingScope SharingScope() => constructors[0].Value.SharingScope();

    public bool ValidForRequest(IBindingRequest request) => constructors.Any(i => i.Value.ValidForRequest(request));
}