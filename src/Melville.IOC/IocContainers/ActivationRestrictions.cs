using System;
using System.Linq;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers
{
    public class ParameterNameCondition: ForwardingActivationStrategy
    {
        private readonly string name;
        public ParameterNameCondition(IActivationStrategy innerActivationStrategy, string name) : base(innerActivationStrategy) => this.name = name;

        public override bool ValidForRequest(IBindingRequest request) => 
            base.ValidForRequest(request) && request.TargetParameterName.Equals(name, StringComparison.Ordinal);
    }
    public class TargetTypeCondition: ForwardingActivationStrategy
    {
        private readonly Type? targetType;
        public TargetTypeCondition(IActivationStrategy innerActivationStrategy, Type? targetType) : base(innerActivationStrategy) =>
            this.targetType = targetType;
        
        public override bool ValidForRequest(IBindingRequest request) => 
            base.ValidForRequest(request) && request.TypeBeingConstructed == targetType;
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