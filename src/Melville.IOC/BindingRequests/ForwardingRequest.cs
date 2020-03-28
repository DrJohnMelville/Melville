using System;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests
{
    public class ForwardingRequest : IBindingRequest
    {
        private readonly IBindingRequest inner;

        public ForwardingRequest(IBindingRequest inner)
        {
            this.inner = inner;
        }

        public virtual Type DesiredType => inner.DesiredType;
        public virtual string TargetParameterName => inner.TargetParameterName;
        public virtual Type? TypeBeingConstructed => inner.TypeBeingConstructed;

        public virtual IIocService IocService
        {
            get => inner.IocService;
            set => inner.IocService = value;
        }

        public object?[] ArgumentsFromParent => inner.ArgumentsFormChild;
        public object?[] ArgumentsFormChild { get; set;} = Array.Empty<object>();
        public virtual bool HasDefaultValue(out object? value)
        {
            value = null;
            return false;
        }
    }
}