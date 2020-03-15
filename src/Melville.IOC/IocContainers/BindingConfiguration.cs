using System;

namespace Melville.IOC.IocContainers
{
    public interface IPickBindingTarget
    {
        IActivationOptions DoBinding(IActivationStrategy strategy);
        IActivationOptions ToType(Type type) =>
            DoBinding(TypeActivatorFactory.CreateTypeActivator(type));
        IActivationOptions ToMethod(Func<IIocService, IBindingRequest, object> method) =>
            DoBinding(new MethodActivationStrategy<object>(method));
        IActivationOptions ToMethod(Func<IIocService, object> method) =>
            DoBinding(new MethodActivationStrategy<object>((s,r)=>method(s)));
        IActivationOptions ToMethod(Func<object> method) =>
            DoBinding(new MethodActivationStrategy<object>((s,r)=>method()));
    }
    public interface IPickBindingTarget<TSource>: IPickBindingTarget
    {
        IActivationOptions To<TDestination>() where TDestination : TSource;
        IActivationOptions ToConstant(TSource targetObject) => 
            DoBinding(new ConstantActivationStrategy(targetObject!));
        IPickBindingTarget<TSource> And<TDestination>();
        IActivationOptions ToSelf() => To<TSource>();
         IActivationOptions ToMethod(Func<IIocService, IBindingRequest, TSource> method) =>
            DoBinding(new MethodActivationStrategy<TSource>(method));
         IActivationOptions ToMethod(Func<IIocService, TSource> method) =>
            DoBinding(new MethodActivationStrategy<TSource>((s,r)=>method(s)));

         IActivationOptions ToMethod(Func<TSource> method) =>
             DoBinding(new MethodActivationStrategy<TSource>((s, r) => method()));
    }
    public class BindingConfiguration: IPickBindingTarget
    {
        private readonly Type targetType;
        protected BindingRegistry Registry { get; }
        protected bool IfNeeded { get; }

        public BindingConfiguration(Type targetType, BindingRegistry registry, bool ifNeeded)
        {
            this.targetType = targetType;
            Registry = registry;
            IfNeeded = ifNeeded;
        }

        public IActivationOptions DoBinding(IActivationStrategy strategy) => 
            Registry.Bind(targetType, strategy, IfNeeded);
    }
    public class BindingConfiguration<TSource> : BindingConfiguration, IPickBindingTarget<TSource>
    {

        public BindingConfiguration(BindingRegistry registry, bool ifNeeded):base(typeof(TSource), registry,
            ifNeeded)
        {
        }

        public IActivationOptions To<TDestination>() where TDestination : TSource
        {
            return ((IPickBindingTarget)this).ToType(typeof(TDestination));
        }

        public IPickBindingTarget<TSource> And<TDestination>() =>
        new MultiBindingConfiguration<TSource>(Registry, IfNeeded).And<TDestination>();

    }
}