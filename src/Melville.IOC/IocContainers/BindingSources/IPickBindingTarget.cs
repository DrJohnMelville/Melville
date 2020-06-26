using System;
using System.Collections.Generic;
using System.Reflection;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;

namespace Melville.IOC.IocContainers.BindingSources
{
    public abstract class IPickBindingTarget<TSource>
    {
        public IActivationOptions<TSource> ToType(Type type, Func<IList<ConstructorInfo>, IActivationStrategy> picker) =>
            DoBinding(TypeActivatorFactory.CreateTypeActivator(type, picker));
        public IActivationOptions<TSource> ToType(Type type, ConstructorSelector picker) =>
            ToType(type, i => picker(i).AsActivationStrategy());
        public IActivationOptions<TSource> ToType(Type type) => ToType(type, ConstructorSelectors.MaximumArgumentCount);
        public IActivationOptions<TSource> ToConstant(TSource targetObject) => 
            DoBinding(new ConstantActivationStrategy(targetObject!));
        public IActivationOptions<TSource> ToSelf() => To<TSource>();
        public IActivationOptions<TSource> ToSelf(Func<IList<ConstructorInfo>, IActivationStrategy> pickConstructor)
            => To<TSource>(pickConstructor);
        public IActivationOptions<TSource> ToSelf(ConstructorSelector pickConstructor)
            => To<TSource>(pickConstructor);
        public IActivationOptions<TSource> ToMethod(Func<IIocService, IBindingRequest, TSource> method) =>
            DoBinding(new MethodActivationStrategy<TSource>(method));

        public IActivationOptions<TSource> ToMethod(Func<IIocService, TSource> method) =>
            DoBinding(new MethodActivationStrategy<TSource>((s, r) => method(s)));

        public IActivationOptions<TSource> ToMethod(Func<TSource> method) =>
            DoBinding(new MethodActivationStrategy<TSource>((s, r) => method()));
        
        public abstract IActivationOptions<TSource> DoBinding(IActivationStrategy strategy);
        public abstract IPickBindingTarget<TSource> And<TDestination>();

        public IActivationOptions<TSource> To<TDestination>(
            Func<IList<ConstructorInfo>, IActivationStrategy> pickConstructor) where TDestination : TSource =>
            DoBinding(TypeActivatorFactory.CreateTypeActivator(typeof(TDestination), pickConstructor));
        public IActivationOptions<TSource> To<TDestination>() where TDestination : TSource =>
            To<TDestination>(ConstructorSelectors.MaximumArgumentCount);// default policy is to pick the longest
        public IActivationOptions<TSource> To<TDestination>(ConstructorSelector pickConstructor) 
            where TDestination : TSource =>
            To<TDestination>(i => pickConstructor(i).AsActivationStrategy());
    }
}