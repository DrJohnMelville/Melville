using System;

namespace Melville.IOC.BindingRequests;

public class ParameterizedRequest : TypeChangeBindingRequest
{ 
    public ParameterizedRequest(IBindingRequest inner, Type targetType, 
        object[] parameters) : base(inner, targetType)
    {
        ArgumentsFormChild = parameters;
    }
}