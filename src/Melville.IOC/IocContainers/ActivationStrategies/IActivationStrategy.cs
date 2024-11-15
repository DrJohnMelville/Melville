using System;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public interface  IActivationStrategy
{
    bool CanCreate(IBindingRequest bindingRequest);
    object? Create(IBindingRequest bindingRequest);
    SharingScope SharingScope();
    bool ValidForRequest(IBindingRequest request);
    void CreateMany(IBindingRequest bindingRequest, Func<object?, int> accumulator)
    {
        var ret = Create(bindingRequest);
        if (bindingRequest.IsCancelled)
        {
            if (ret is IDisposable disp) disp.Dispose();
        }
        if (ret is null) return;
        accumulator(Create(bindingRequest) );
    }
}