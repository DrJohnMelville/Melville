using System;
using System.Linq;
using System.Reflection;
using Melville.Hacks;
using Melville.INPC;
using Melville.IOC.BindingRequests;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public partial class TypeActivationStrategy: IActivationStrategy
{
    [FromConstructor] private readonly ConstructorInvoker activator;
    [FromConstructor] private readonly ParameterInfo[] paramTypes;

    public SharingScope SharingScope() => IocContainers.SharingScope.Transient;
    public bool ValidForRequest(IBindingRequest request) => true;
    public bool CanCreate(IBindingRequest bindingRequest)
    {
        return bindingRequest.IocService.CanGet(paramTypes.Select(bindingRequest.CreateSubRequest));
    }


    public object? Create(IBindingRequest bindingRequest)
    {
        bindingRequest.IocService.Debugger.ActivationRequested(bindingRequest);
        using var requests = new RentedBuffer<object?>(paramTypes.Length);
        FillParamaterSpan(bindingRequest, requests.Span);
        if (bindingRequest.IsCancelled)
        {
            foreach (var item in requests.Span)
            {
                if (item is IDisposable disp) disp.Dispose();
            }
            return null;
        }
        return activator.Invoke(requests.Span).TryRegisterDisposeAndReturn(bindingRequest);
    }

    private void FillParamaterSpan(IBindingRequest bindingRequest, Span<object?> target)
    {
        bindingRequest.IocService.Fill(target, 
            paramTypes.Select(bindingRequest.CreateSubRequest));
     }
}