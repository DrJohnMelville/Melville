using System;
using System.Linq;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers
{
    public class ObjectFactory: ForwardingActivationStrategy
    {
        public static ObjectFactory? ForceToObjectFactory(IActivationStrategy? activationStrategy)
        {
            return activationStrategy switch
            {
                null => null,
                ObjectFactory factory  => factory,
                _=> new ObjectFactory(activationStrategy)
            };
        }
        public ObjectFactory(IActivationStrategy innerActivationStrategy) :
            base(new AttemptDisposeRegistration(innerActivationStrategy))
        {
        }
    }

    public class ObjectFactory<T>: ObjectFactory, IActivationOptions<T>
    {


        public ObjectFactory(IActivationStrategy innerActivationStrategy):base(innerActivationStrategy)
        {
        }

        public IActivationOptions<T> AddActivationStrategy(Func<IActivationStrategy, IActivationStrategy> newStrategy)
        {
            InnerActivationStrategy = newStrategy(InnerActivationStrategy);
            return this;
        }
        
        public IActivationOptions<T> WrapWith<TWrapper>() where TWrapper : T =>
            ((IActivationOptions<T>)this).WrapWith((item, request) => GetWrappedItem<TWrapper>(request,
                    new object[]{item?? throw new IocException("Cannot wrap a null object")})
            );
        
        public IActivationOptions<T> WrapWith<TWrapper>(params  object[] parameters) where TWrapper : T =>
            ((IActivationOptions<T>)this).WrapWith((item, request) => GetWrappedItem<TWrapper>(request, 
                PrependItem(parameters, item?? throw new IocException("Cannot wrap a null object"))));
        

        private static T GetWrappedItem<TWrapper>(IBindingRequest request, object[] parameters) where TWrapper : T
        {
            return (T) request.IocService.Get(request.CreateSubRequest(typeof(TWrapper)!, parameters));
        }

        private static object[] PrependItem(object[] parameters, T item)
        {
            return (parameters??Array.Empty<object>()).Prepend(item
                  ?? throw new IocException("Cannot wrap a null object")).ToArray();
        }
    }

}