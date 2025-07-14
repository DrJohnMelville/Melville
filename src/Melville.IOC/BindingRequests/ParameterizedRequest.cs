using System;
using System.Collections.Generic;
using System.Linq;

namespace Melville.IOC.BindingRequests;

public class ParameterizedRequest : TypeChangeBindingRequest
{ 
    public ParameterizedRequest(IBindingRequest inner, Type targetType, 
        object[] arguments) : base(inner, targetType)
    {
        Arguments = arguments.Length>0? arguments.Concat(inner.Arguments) : inner.Arguments;
    }

    public override IEnumerable<object> Arguments { get; }
}