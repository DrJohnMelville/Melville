using System;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests;

public class RootBindingRequest : IBindingRequest
{
    public RootBindingRequest(Type targetType, IIocService iocService, params object[] arguments)
    {
        DesiredType = targetType;
        IocService = iocService;
        ArgumentsFormChild = arguments;
    }
    public Type DesiredType { get; }
    public string TargetParameterName => "!Root Request!";
    public IIocService IocService { get; set; }
    public Type? TypeBeingConstructed => null;
    public object?[] ArgumentsFormChild { get; set;}
    public object?[] ArgumentsFromParent => Array.Empty<object>();

    public bool IsCancelled { get; set; }

    public IBindingRequest? Parent => null;
}