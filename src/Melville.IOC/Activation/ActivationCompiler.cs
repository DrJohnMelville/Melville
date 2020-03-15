using System;
using System.Reflection;
using System.Reflection.Emit;
using Melville.IOC.IocContainers;

namespace Melville.IOC.Activation
{
    public static class ActivationCompiler
    {
        public static Func<object?[], object> Compile(Type targetType, params Type[] parameters) =>
            Compile(targetType, targetType.GetConstructor(parameters)??
                                throw new IocException("No constructor found for these parameters."));

        public static Func<object?[], object> Compile(Type targetType, ConstructorInfo constructor)
        {
            ActivatableTypesPolicy.ThrowIfNotActivatable(targetType);
            var method = new DynamicMethod("CreateInstance", typeof(object), new []{typeof(object[])});
            EmitCreationMethod(targetType, constructor, method.GetILGenerator());
            return AssembleToDelegate(method);
        }

        private static void EmitCreationMethod(Type targetType, ConstructorInfo constructor, ILGenerator il)
        {
            EmitParameters(il, constructor.GetParameters());
            EmitObjectCreationAndExit(constructor, il, targetType);
        }

        private static Func<object?[], object> AssembleToDelegate(DynamicMethod method) => 
            (Func<object?[], object>) method.CreateDelegate(typeof(Func<object[], object>));

        private static void EmitParameters(ILGenerator il, ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                EmitArrayElementToStack(il, i);
                TryUnboxValueTypes(il, parameters, i);
            }
        }

        private static void TryUnboxValueTypes(ILGenerator il, ParameterInfo[] parameters, int i)
        {
            if (parameters[i].ParameterType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, parameters[i].ParameterType);
            }
        }

        private static void EmitArrayElementToStack(ILGenerator il, int i)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Ldelem_Ref);
        }

        private static void EmitObjectCreationAndExit(ConstructorInfo constructor, ILGenerator il, Type targetType)
        {
            il.Emit(OpCodes.Newobj, constructor);
            if (targetType.IsValueType)
            {
                il.Emit(OpCodes.Box, targetType);
            }
            il.Emit(OpCodes.Ret);
        }
    }
}