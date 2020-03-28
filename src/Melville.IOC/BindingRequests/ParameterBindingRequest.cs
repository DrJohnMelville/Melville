using System;
using System.Reflection;

namespace Melville.IOC.BindingRequests
{
    public class ParameterBindingRequest : ForwardingRequest
    {
        private readonly ParameterInfo parameter;
        public ParameterBindingRequest(ParameterInfo parameter, IBindingRequest parentRequest): base(parentRequest)
        {
            this.parameter = parameter;
        }
        public override Type DesiredType => parameter.ParameterType;
        public override string TargetParameterName => parameter.Name ?? "";
        public override Type? TypeBeingConstructed => parameter.Member.DeclaringType;

        public override bool HasDefaultValue(out object? value)
        {
            value = parameter.DefaultValue;
            return parameter.HasDefaultValue;
        }
    }
}