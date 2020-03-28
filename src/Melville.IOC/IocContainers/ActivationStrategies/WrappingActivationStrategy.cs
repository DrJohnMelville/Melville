using System;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public class WrappingActivationStrategy<T> : ForwardingActivationStrategy
    {
        private readonly Func<T, IBindingRequest, T> wrapperFunc;

        public WrappingActivationStrategy(IActivationStrategy parent, 
            Func<T,IBindingRequest,T> wrapperFunc) : base(parent)
        {
            this.wrapperFunc = wrapperFunc;
        }

        public override object? Create(IBindingRequest bindingRequest)
        {
            var ret = (T) (InnerActivationStrategy.Create(bindingRequest) ??
                throw new IocException("Wrapped Activation Strategy returned null"));
            return wrapperFunc(ret, bindingRequest);
        }

        public override SharingScope SharingScope() => 
            IocContainers.SharingScope.Transient;
    }
    
    //There is a bug here that I am not going to fix.  Of we were to make the wrapper
    // scoped while it was singleton in the original context then we could
    // prematurely dispose of the wrapper because we do not flow the wrapper's scope
    // into the present oboject.  Most wrappers should be transients in the underlying 
    // contaainer anyway.
}