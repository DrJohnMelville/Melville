using System;
using System.Linq;
using System.Reflection;
using Melville.Hacks;
using Melville.INPC;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public partial class TypeActivationStrategy: IActivationStrategy
{
    [FromConstructor] private readonly ConstructorInvoker activator;
    [FromConstructor] private readonly ParameterInfo[] paramTypes;

    public SharingScope SharingScope() => IocContainers.SharingScope.Transient;
    public bool ValidForRequest(IBindingRequest request) => true;
    public bool CanCreate(IBindingRequest bindingRequest)
    {
        foreach (var parameterInfo in paramTypes)
        {
            if (!bindingRequest.IocService.CanGet(bindingRequest.CreateSubRequest(parameterInfo)))
                return false;
        }
        return true;
    }

    public object? Create(IBindingRequest bindingRequest)
    {
        using var requests = new RentedBuffer<object?>(paramTypes.Length);
        FillParamaterSpan(bindingRequest, requests.Span);
        return activator.Invoke(requests.Span);
    }

    private void FillParamaterSpan(IBindingRequest bindingRequest, Span<object?> target)
    {
        bindingRequest.IocService.Fill(target, 
            paramTypes.Select(bindingRequest.CreateSubRequest));
     }
}