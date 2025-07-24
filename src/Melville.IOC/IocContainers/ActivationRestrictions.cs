using System;
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
    
public class AddParametersStrategy(IActivationStrategy innerActivationStrategy, object[] parameters)
    : ForwardingActivationStrategy(innerActivationStrategy)
{
    public override object? Create(IBindingRequest bindingRequest) => 
        base.Create(bindingRequest.CreateSubRequest(bindingRequest.DesiredType, parameters));

    public override bool CanCreate(IBindingRequest bindingRequest) => 
        base.CanCreate(bindingRequest.CreateSubRequest(bindingRequest.DesiredType, parameters));
}