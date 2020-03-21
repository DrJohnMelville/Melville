using System;
using System.Linq;
using System.Threading.Tasks;
using Melville.IOC.Activation;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.TypeResolutionPolicy
{
    public class AsyncFunctionActivationStrategy: IActivationStrategy
    {
        private readonly Type innerFunctionType;
        private readonly Type targetObjectType;
        private readonly ForwardFuncToMethodCall staticCreatorFactory;
        private readonly ForwardFuncToMethodCall asyncInitializerFactory;

        public AsyncFunctionActivationStrategy(Type outerFunctionType)
        {
            innerFunctionType = ReturnType(outerFunctionType);
            targetObjectType = GetTypeFromFuncOrTask(innerFunctionType);
            if (IsTask(innerFunctionType))
            {
                staticCreatorFactory = asyncInitializerFactory
                    = new ForwardFuncToMethodCall(outerFunctionType,
                        nameof(AsyncFunctionFactoryStub.CallStaticFunc),
                        typeof(AsyncFunctionFactoryStub));
            }else{
              staticCreatorFactory = new ForwardFuncToMethodCall(outerFunctionType,
                nameof(AsyncFunctionFactoryStub.CreateAsyncInitializerFunc),
                typeof(AsyncFunctionFactoryStub));
              asyncInitializerFactory = MakeAsyncInitializerFactory();
            }
        }
        private ForwardFuncToMethodCall MakeAsyncInitializerFactory()
        {
            var retType = typeof(AsyncFunctionFactoryImplementation<>).MakeGenericType(targetObjectType);
            return new ForwardFuncToMethodCall(innerFunctionType,
                nameof(AsyncFunctionFactoryImplementation<object>.Create), retType);
        }

        private Type GetTypeFromFuncOrTask(Type type) => GetTypeFromTask(IsTask(type)?type:ReturnType(type));

        private static Type GetTypeFromTask(Type type) => type.GetGenericArguments().Single();

        private static Type ReturnType(Type functionType) => functionType.GetGenericArguments().Last();
        private static bool IsTask(Type type) => type.GetGenericTypeDefinition() == typeof(Task<>);

        public bool CanCreate(IBindingRequest bindingRequest) => true;

        public object? Create(IBindingRequest bindingRequest) =>
            CreateOutputObject(new AsyncFunctionFactoryStub(bindingRequest,
                targetObjectType, asyncInitializerFactory));

        protected virtual object CreateOutputObject(AsyncFunctionFactoryStub stub) => 
            staticCreatorFactory.CreateFuncDelegate(stub);

        public SharingScope SharingScope() => IocContainers.SharingScope.Transient;
        public bool ValidForRequest(IBindingRequest request) => true;
    }
    
    public class AsyncFunctionFactoryStub
    {
        private readonly IBindingRequest request;
        private readonly Type targetObjectType;
        private readonly ForwardFuncToMethodCall asyncInitializerFactory;

        public AsyncFunctionFactoryStub(IBindingRequest request, Type targetObjectType, 
            ForwardFuncToMethodCall asyncInitializerFactory)
        {
            this.request = request;
            this.targetObjectType = targetObjectType;
            this.asyncInitializerFactory = asyncInitializerFactory;
        }

        public object? CallStaticFunc(object[] parameters) => 
            GetFactoryImplementation().CreateStatic(parameters, request);


        public object? CreateAsyncInitializerFunc(object[] parameters)
        {
            var factoryImplementation1 = GetFactoryImplementation();
            factoryImplementation1.SetRequest(request.CreateSubRequest(targetObjectType, parameters));
            var factoryImplementation = factoryImplementation1;
            return asyncInitializerFactory.CreateFuncDelegate(factoryImplementation);
        }

        private IAsyncFunctionFactoryImplementation GetFactoryImplementation()
        {
            var runnerType = typeof(AsyncFunctionFactoryImplementation<>).MakeGenericType(targetObjectType);
            var creationRunner = (IAsyncFunctionFactoryImplementation)
                (request.IocService.Get(request.CreateSubRequest(runnerType)) ??
                throw new InvalidOperationException("Could not retrieve Factory Implementation from IOC"));
            return creationRunner;
        }

        public void ClearCache() => GetFactoryImplementation().ClearCache();
    }
}