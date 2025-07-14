using System;
using System.Linq;
using Melville.INPC;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests;

public partial class ClonedBindingRequest(IBindingRequest rootRequest) : IBindingRequest
{
    [DelegateTo] public IBindingRequest Parent { get; } = rootRequest;

    public override string ToString() => Trace;
}