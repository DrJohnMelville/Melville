using System;
using System.Linq;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy;

public class FunctionsIntoFactories : ITypeResolutionPolicy
{
    public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) =>
        ApplyResolutionPolicy(request.DesiredType);

    public IActivationStrategy? ApplyResolutionPolicy(Type t) =>
        IsFunction(t) ? new FunctionActivationStrategy(t) : null;


    private bool IsFunction(Type type) =>
        type.IsConstructedGenericType &&
        VerifyGenericTypeDeclaration(type.GetGenericTypeDefinition());

    private bool VerifyGenericTypeDeclaration(Type type)
    {
        return type == FunctionDefinitions[type.GetGenericArguments().Length -1];
    }

    private static readonly Type[] FunctionDefinitions = {
        typeof(Func<>),
        typeof(Func<,>),
        typeof(Func<,,>),
        typeof(Func<,,,>),
        typeof(Func<,,,,>),
        typeof(Func<,,,,,>),
        typeof(Func<,,,,,,>),
        typeof(Func<,,,,,,,>),
        typeof(Func<,,,,,,,,>),
        typeof(Func<,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,,,,>),
    };
}