using System;
using System.Linq;
using System.Threading;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers
{
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
            bindingRequest.ExtraParamsForChild = 
                bindingRequest.ExtraParamsForChild.Concat(parameters).ToArray();
            return base.Create(bindingRequest);
        }
    }
}