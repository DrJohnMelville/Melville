using System;
using System.Reflection;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests
{
    public interface IBindingRequest
    {
        Type DesiredType { get; }
        Type? TypeBeingConstructed { get; }
        string TargetParameterName { get; }
        IIocService IocService { get; set; }
        IBindingRequest CreateSubRequest(ParameterInfo info)=> new ParameterBindingRequest(info, this);
        IBindingRequest CreateSubRequest(Type type)=> new TypeChangeBindingRequest(this, type);
        IBindingRequest CreateSubRequest(Type type, params object[] parameters)=> 
            new ParameterizedRequest(this, type, parameters);
        IBindingRequest Clone() => new ClonedBindingRequest(this);

        bool HasDefaultValue(out object? value)
        {
            value = null;
            return false;
        }

        object?[] ArgumentsFormChild { get; set; }
        object?[] ArgumentsFromParent { get; }
    }
}