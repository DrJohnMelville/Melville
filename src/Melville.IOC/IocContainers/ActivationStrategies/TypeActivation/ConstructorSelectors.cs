using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.IOC.Activation;

namespace Melville.IOC.IocContainers.ActivationStrategies.TypeActivation
{
    public delegate ConstructorInfo ConstructorSelector(IList<ConstructorInfo> constrctors);
    public static class ConstructorSelectors
    {
        public static IActivationStrategy EmulateDotNet(this IList<ConstructorInfo> constructors) =>
            constructors.Count switch
            {
                1 => AsActivationStrategy(constructors[0]),
                _ => new MultiConstructorStrategy(constructors)
            };
        public static ConstructorInfo SingleConstructor(this IList<ConstructorInfo> constructors) =>
            constructors.Single();
        public static ConstructorInfo MaximumArgumentCount(this IList<ConstructorInfo> constructors) =>
            constructors
                .GroupBy(i=>i.GetParameters().Length)  // Group constructors by argument count
                .OrderByDescending(i=>i.Key).First() // group with the longets list
                .Single();  // must have a single member.
        public static TypeActivationStrategy AsActivationStrategy(this ConstructorInfo constructor)
        {
            return new TypeActivationStrategy(
                ActivationCompiler.Compile(constructor), constructor.GetParameters());
        }
        public static ConstructorInfo WithArgumentTypes(this IList<ConstructorInfo> constructors,
            params Type[] argumentTypes)
        {
            return constructors.Single(i =>
            {
                var parameterTypes = i.GetParameters().Select(i => i.ParameterType).ToList();
                return parameterTypes.Count == argumentTypes.Length &&
                       parameterTypes.Zip(argumentTypes, (j,k) => j==k)
                           .All(k=>k);
            });
        }


        public static ConstructorInfo DefaultConstructor(this IList<ConstructorInfo> constructors) =>
            constructors.WithArgumentTypes();
        public static ConstructorInfo WithArgumentTypes<T1>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1));

        public static ConstructorInfo WithArgumentTypes<T1, T2>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7, T8>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));

        public static ConstructorInfo WithArgumentTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IList<ConstructorInfo> constructors) => 
            constructors.WithArgumentTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16));
    }
}