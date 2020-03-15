using System;
using Melville.IOC.Activation;

namespace Melville.IOC.TypeResolutionPolicy
{
    public class ClearableAsyncFunctionActivationStrategy : AsyncFunctionActivationStrategy
    {
        private Func<object?[], object> tupleCreator;
        public ClearableAsyncFunctionActivationStrategy(Type resultTupleType) : 
            base(GetCreatorFunctionType(resultTupleType))
        {
            tupleCreator = ActivationCompiler.Compile(resultTupleType,
                typeof(Action), GetCreatorFunctionType(resultTupleType));
        }

        private static Type GetCreatorFunctionType(Type outerFunctionType) => 
            outerFunctionType.GetGenericArguments()[1];

        protected override object CreateOutputObject(AsyncFunctionFactoryStub stub)
        {
            return tupleCreator(new[]
            {
                (Action) stub.ClearCache,
                base.CreateOutputObject(stub)
            });
        }
    }
}