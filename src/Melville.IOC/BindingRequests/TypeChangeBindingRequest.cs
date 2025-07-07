using System;

namespace Melville.IOC.BindingRequests;

public class TypeChangeBindingRequest : ForwardingRequest
{
    public TypeChangeBindingRequest(IBindingRequest inner, Type targetType): base(inner)
    {
        this.DesiredType = targetType;
    }
    public override Type DesiredType { get; }
}
