﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers
{
    //IActivationOptions is a fluent interface to configure object creation.  The interface is strongly typed to
    // the kind of value being created.  This allows the wrapping APIs to be strongly typed.
    //
    //  There are a few places, notably in the registration of open generics, where I do not statically know the
    // type of the item to be created.  Since I do not know they static type I creat a ITypesafeActivationOptions<object>
    // The ItypesafeActivationOptions contains all the methods you can safely use in this context, that is all the
    // methods where the only use of T is to return this with a strongly typed value.  In those places, we can
    // use the less generous interface to avoid surfacing type errors.
    public interface ITypesafeActivationOptions<T>
    {
        IActivationOptions<T> AddActivationStrategy(Func<IActivationStrategy, IActivationStrategy> newStrategy);

        ObjectFactory GetFinalFactory();
        // sharing scopes
        IActivationOptions<T> AsSingleton() => AddActivationStrategy(SingletonActivationStrategy.EnsureSingleton);
        IActivationOptions<T> AsScoped() => AddActivationStrategy(i => new ScopedActivationStrategy(i));

        // Disposal scopes
        IActivationOptions<T> DoNotDispose() => AddActivationStrategy(i => new ForbidDisposalStrategy(i,true));
        IActivationOptions<T> DisposeIfInsideScope() => AddActivationStrategy(i => new ForbidDisposalStrategy(i, false));

        // Injection restrictions
        IActivationOptions<T> When(Func<IBindingRequest, bool> predicate) =>
            AddActivationStrategy(i => new LambdaCondition(i, predicate));
        IActivationOptions<T> InNamedParameter(string name) =>
            When(p => name.Equals(p.TargetParameterName, StringComparison.Ordinal));
        IActivationOptions<T> WhenConstructingType(Type type) =>
            When(p => type.IsAssignableFrom(p.TypeBeingConstructed));
        IActivationOptions<T> WhenConstructingType<TTarget>() => WhenConstructingType(typeof(TTarget));
        IActivationOptions<T> WhenNotConstructingType(Type type) =>
            When(p => p.TypeBeingConstructed == null || !type.IsAssignableFrom(p.TypeBeingConstructed));
        IActivationOptions<T> WhenNotConstructingType<TTarget>() => WhenNotConstructingType(typeof(TTarget));
        IActivationOptions<T> BlockSelfInjection() => WhenNotConstructingType(typeof(T));
        IActivationOptions<T> WhenParameterHasValue(object value) => When(i => i.ArgumentsFormChild.Contains(value));
        IActivationOptions<T> WhenParameterHasValue(string value, StringComparison compareType) =>
            When(i => 
                i.ArgumentsFormChild
                    .OfType<string>()
                    .Any(j => j.Equals(value, compareType)));
        IActivationOptions<T> WhenParameterHasType<TParameter>() => WhenParameterHasType(typeof(TParameter));

        IActivationOptions<T> WhenParameterHasType(Type parameterType) =>
            When(i => i.ArgumentsFormChild.Any(parameterType.IsInstanceOfType));

        
        // Additional construction info.
        IActivationOptions<T> WithParameters(params object[] parameters) => AddActivationStrategy(i =>
            new AddParametersStrategy(i, parameters));

    }
    public interface IActivationOptions<T>:ITypesafeActivationOptions<T>
    {
        IActivationOptions<T> FixResult(Action<T> act) => FixResult((i, j) => act(i));
        IActivationOptions<T> FixResult(Action<T, IBindingRequest> act) => 
          WrapWith((item, bindingRequest) =>
          {
              act(item, bindingRequest);
              return item;
          });
        IActivationOptions<T> WrapWith(Func<T, T> wrapper) => WrapWith((i, j) => wrapper(i));
        IActivationOptions<T> WrapWith(Func<T, IBindingRequest, T> wrapper) =>
            AddActivationStrategy(i => new WrappingActivationStrategy<T>(i, wrapper));

        IActivationOptions<T> WrapWith<TWrapper>() where TWrapper : T;
        IActivationOptions<T> WrapWith<TWrapper>(params object[] parameters) where TWrapper : T;

        IActivationOptions<T> RegisterWrapperForDisposal() =>
            AddActivationStrategy(i => new AttemptDisposeRegistration(i));
    }
}