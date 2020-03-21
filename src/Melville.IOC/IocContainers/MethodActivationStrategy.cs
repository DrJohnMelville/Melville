using System;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers
{
    public class MethodActivationStrategy<T>: IActivationStrategy
    {
        private readonly Func<IIocService, IBindingRequest, T> method;
        public bool ValidForRequest(IBindingRequest request) => true;
        public MethodActivationStrategy(Func<IIocService, IBindingRequest, T> method)
        {
            this.method = method;
        }

        // we have to assume this works
        public bool CanCreate(IBindingRequest bindingRequest) => true;

        public (object? Result, DisposalState DisposalState) Create(IBindingRequest bindingRequest)=>
            (method(bindingRequest.IocService, bindingRequest), DisposalState.DisposalRequired);

        public SharingScope SharingScope() => IocContainers.SharingScope.Transient;
    }
}