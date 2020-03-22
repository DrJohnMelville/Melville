using System;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers
{
    public interface IPickBindingTarget<TSource>
    {
        IActivationOptions<TSource> DoBinding(IActivationStrategy strategy);
        IActivationOptions<TSource> ToType(Type type) =>
            DoBinding(TypeActivatorFactory.CreateTypeActivator(type));
        IActivationOptions<TSource> ToConstant(TSource targetObject) => 
            DoBinding(new ConstantActivationStrategy(targetObject!));
        IPickBindingTarget<TSource> And<TDestination>();
        IActivationOptions<TSource> ToSelf() => To<TSource>();
         IActivationOptions<TSource> ToMethod(Func<IIocService, IBindingRequest, TSource> method) =>
            DoBinding(new MethodActivationStrategy<TSource>(method));
         IActivationOptions<TSource> ToMethod(Func<IIocService, TSource> method) =>
            DoBinding(new MethodActivationStrategy<TSource>((s,r)=>method(s)));

         IActivationOptions<TSource> ToMethod(Func<TSource> method) =>
             DoBinding(new MethodActivationStrategy<TSource>((s, r) => method()));
         
         // for reasons I do not completely understand, defining this method in the interface
         // causes a runtime type constraint verification exception when TSource == TDestination
         // The verifier is happy when the method is defined in the classes however.
         IActivationOptions<TSource> To<TDestination>() where TDestination : TSource;
    }

    public class BindingConfiguration<TSource>: IPickBindingTarget<TSource>
    {
        private readonly Type targetType;
        protected BindingRegistry Registry { get; }
        protected bool IfNeeded { get; }

        public BindingConfiguration(BindingRegistry registry, bool ifNeeded):
            this(typeof(TSource), registry, ifNeeded){}
        
        public BindingConfiguration(Type targetType, BindingRegistry registry, bool ifNeeded)
        {
            this.targetType = targetType;
            Registry = registry;
            IfNeeded = ifNeeded;
        }

        public IActivationOptions<TSource> DoBinding(IActivationStrategy strategy) => 
            Registry.Bind<TSource>(targetType, strategy, IfNeeded);

        public IActivationOptions<TSource> To<TDestination>() where TDestination : TSource => 
            ((IPickBindingTarget<TSource>)this).ToType(typeof(TDestination));

        public IPickBindingTarget<TSource> And<TDestination>() =>
        new MultiBindingConfiguration<TSource>(Registry, IfNeeded).And<TDestination>();

    }
}