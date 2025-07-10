using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.INPC;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.TypeResolutionPolicy;

public partial class FunctionActivationStrategy : IActivationStrategy
{
    [FromConstructor] private readonly Type functionDelegateType;

    public bool CanCreate(IBindingRequest bindingRequest) => true;

    private IBindingRequest InnerRequestForCanCreate(IBindingRequest bindingRequest)
    {
        var types = functionDelegateType.GetGenericArguments().ToList();
        return bindingRequest.CreateSubRequest(types.Last(),
            CreateFakeObjectsFromFunctionParameters(types).ToArray());
    }

    private static IEnumerable<object> CreateFakeObjectsFromFunctionParameters(List<Type> types) => 
        types.SkipLast(1).Select(i => (object) new ReplaceLiteralArgumentWithNonsenseValue(i));

    public object? Create(IBindingRequest bindingRequest)
    {
        var genericArgs = functionDelegateType.GetGenericArguments();
        return Delegate.CreateDelegate(functionDelegateType, 
            new FunctionFactoryImplementation(bindingRequest, genericArgs[^1]), 
            CreateSpecializedMethod(genericArgs)
        );
    }

    private static MethodInfo CreateSpecializedMethod(Type[] genericArgs) =>
        typeof(FunctionFactoryImplementation)
            .GetMethods()
            .First(i => i.Name is nameof(FunctionFactoryImplementation.Call) &&
                        i.GetGenericArguments().Length == genericArgs.Length)
            .MakeGenericMethod(genericArgs);

    public SharingScope SharingScope() => IocContainers.SharingScope.Transient;

    public bool ValidForRequest(IBindingRequest ret) => true;
}