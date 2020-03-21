using System;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public interface  IActivationStrategy
    {
        bool CanCreate(IBindingRequest bindingRequest);
        (object? Result, DisposalState DisposalState) Create(IBindingRequest bindingRequest);
        SharingScope SharingScope();
        bool ValidForRequest(IBindingRequest request);
        void CreateMany(IBindingRequest bindingRequest, Func<object?, int> accumulator) =>
            accumulator(Create(bindingRequest).UnwrapCheckNullAndDispose());
    }
}