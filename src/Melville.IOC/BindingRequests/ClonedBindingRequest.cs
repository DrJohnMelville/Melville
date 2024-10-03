using System;
using System.Linq;
using Melville.INPC;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests;

public partial class ClonedBindingRequest(IBindingRequest rootRequest) : IBindingRequest
{
    [DelegateTo] public IBindingRequest Parent { get; } = rootRequest;

    // copy the arrays because they get destroyed
    public object?[] ArgumentsFormChild { get; set; } = rootRequest.ArgumentsFormChild.ToArray();
    public object?[] ArgumentsFromParent { get; } = rootRequest.ArgumentsFromParent.ToArray();
}