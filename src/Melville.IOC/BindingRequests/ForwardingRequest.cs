using System;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests;

public class ForwardingRequest(IBindingRequest inner) : IBindingRequest
{
    public IBindingRequest Parent { get; } = inner;
    public virtual Type DesiredType => Parent.DesiredType;
    public virtual string TargetParameterName => Parent.TargetParameterName;
    public virtual Type? TypeBeingConstructed => Parent.TypeBeingConstructed;

    public virtual IIocService IocService => Parent.IocService;

    public object?[] ArgumentsFromParent => Parent.ArgumentsFormChild;
    public object?[] ArgumentsFormChild { get; set;} = Array.Empty<object>();
    public virtual bool HasDefaultValue(out object? value)
    {
        value = null;
        return false;
    }

    public bool IsCancelled
    {
        get => Parent.IsCancelled;
        set => Parent.IsCancelled = value;
    }

    public string Trace => this.ConstructFailureMessage();
}