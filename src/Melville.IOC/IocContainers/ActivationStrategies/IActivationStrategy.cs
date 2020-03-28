using System;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public interface  IActivationStrategy
    {
        bool CanCreate(IBindingRequest bindingRequest);
        object? Create(IBindingRequest bindingRequest);
        SharingScope SharingScope();
        bool ValidForRequest(IBindingRequest request);
        void CreateMany(IBindingRequest bindingRequest, Func<object?, int> accumulator) =>
            accumulator(Create(bindingRequest) ?? throw new IocException("Type resolved to null"));
    }
}