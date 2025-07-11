﻿using System;
using System.Linq;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers;

public class LambdaCondition: ForwardingActivationStrategy
{
    private readonly Func<IBindingRequest, bool> shouldAllowBinding;

    public LambdaCondition(IActivationStrategy inner, Func<IBindingRequest, bool> shouldAllowBinding): base(inner)
    {
        this.shouldAllowBinding = shouldAllowBinding;
    }

    public override bool ValidForRequest(IBindingRequest request) => 
        base.ValidForRequest(request) && shouldAllowBinding(request);
}
    
public class AddParametersStrategy : ForwardingActivationStrategy
{
    private readonly object[] parameters;
    public AddParametersStrategy(IActivationStrategy innerActivationStrategy, object[] parameters) : base(innerActivationStrategy)
    {
        this.parameters = parameters;
    }

    public override object? Create(IBindingRequest bindingRequest)
    {
        // here we needed to copy the array anyway so that multiple invocations get their own set of
        // variables anyway.  We append our vars to the end of the array so any values provided by a
        // factory will get precedence;
        AugmentParameters(bindingRequest);
        return base.Create(bindingRequest);
    }

#warning -- eventually I need to make ArgumentsFromChild read only lists and save a lot of copies
    //When I do parameterstrategy ises writing the arrays to keep multiple arguments from being 
    // reused, but I could fix this with a separate argument proposing step

    private void AugmentParameters(IBindingRequest bindingRequest)
    {
        bindingRequest.ArgumentsFromChild = [..bindingRequest.ArgumentsFromChild, ..parameters];
    }

    public override bool CanCreate(IBindingRequest bindingRequest)
    {
        AugmentParameters(bindingRequest);
        return base.CanCreate(bindingRequest);
    }
}