using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy
{
    public class FunctionsIntoFactories : ITypeResolutionPolicy
    {
        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) =>
            request.DesiredType switch
            {
                
                var t when IsAsyncFactory(t) => new AsyncFunctionActivationStrategy(t),
                var t when IsClearableAsyncFactory(t) => new ClearableAsyncFunctionActivationStrategy(t),
                var t when IsFunction(t) => new FunctionActivationStrategy(t), 
                _ => null
            };

        private bool IsClearableAsyncFactory(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValueTuple<,>) &&
            HasTupleElementsForCachableAsyncPattern(type.GetGenericArguments());

        private bool HasTupleElementsForCachableAsyncPattern(Type[] elts) => 
            elts[0] == typeof(Action) && IsAsyncFactory(elts[1]);

        private bool IsAsyncFactory(Type staticConstructorType)
        {
            return IsDoubleFunctionStyle(staticConstructorType) || IsSingleFunctionStyle(staticConstructorType);
        }

        private bool IsSingleFunctionStyle(Type staticConstructorType)
        {
            return IsFunctionWithResultType(staticConstructorType, out var asyncInitalizerType) &&
                   IsTaskType(asyncInitalizerType);
        }
        private bool IsDoubleFunctionStyle(Type staticConstructorType)
        {
            return IsFunctionWithResultType(staticConstructorType, out var asyncInitalizerType) &&
                   IsFunctionWithResultType(asyncInitalizerType, out var resultWrapperType) &&
                   IsTaskType(resultWrapperType);
        }

        private bool IsTaskType(Type resultWrapperType) =>
            resultWrapperType.IsGenericType && resultWrapperType.GetGenericTypeDefinition() == typeof(Task<>);
        

        private bool IsFunctionWithResultType(Type input, [NotNullWhen(true)] out Type? returnType)
        {
            returnType = null;
            if (!IsFunction(input)) return false;
            returnType = input.GetGenericArguments().Last();
            return true;

        }
        private bool IsFunction(Type type) =>
            type.IsConstructedGenericType &&
            FunctionDefinitions.Contains(type.GetGenericTypeDefinition());

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
}