using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Melville.IOC.IocContainers;

namespace Melville.IOC.Activation;

public class ForwardFuncToMethodCall
{
    private readonly Type targetType;
    private readonly Type delegateType;
    private readonly DynamicMethod creator;

    public ForwardFuncToMethodCall(Type delegateType, string targetMethod, Type targetType)
    {
        this.delegateType = delegateType;
        this.targetType = targetType;
        creator= CreateInvoker(targetMethod);
    }

    public object CreateFuncDelegate(object methodImplementation) => 
        creator.CreateDelegate(delegateType, methodImplementation);

    private  DynamicMethod CreateInvoker(string targetMethod)
    {
        var argTypes = delegateType.GetGenericArguments()[..^1];
        var method = new DynamicMethod("", FunctionReturnType(), argTypes.Prepend(targetType).ToArray());
        var gen = method.GetILGenerator();

        EmitMethodCall(targetMethod, gen, argTypes);
        gen.Emit(OpCodes.Ret);
        return method;
    }

    private void EmitMethodCall(string targetMethod, ILGenerator gen, Type[] argTypes)
    {
        gen.Emit(OpCodes.Ldarg_0);
        EmitArrayCreation(gen, argTypes);
        EmitFillParameterArrayy(argTypes, gen);
        gen.Emit(OpCodes.Call, FindTargetMethod(targetMethod));
    }

    private MethodInfo FindTargetMethod(string targetMethod)
    {
        return targetType.GetMethod(targetMethod, new[] {typeof(object[])})??
               throw new IocException($"Could not find method {targetMethod} on type {targetType.Name}");
    }

    private static void EmitArrayCreation(ILGenerator gen, Type[] argTypes)
    {
        gen.Emit(OpCodes.Ldc_I4, argTypes.Length);
        gen.Emit(OpCodes.Newarr, typeof(object));
    }

    private void EmitFillParameterArrayy(Type[] argTypes, ILGenerator gen)
    {
        for (int i = 0; i < argTypes.Length; i++)
        {
            EmitSingleParameter(gen, i, argTypes[i]);
        }
    }

    private void EmitSingleParameter(ILGenerator gen, int i, Type argumentType)
    {
        gen.Emit(OpCodes.Dup);
        gen.Emit(OpCodes.Ldc_I4, i);
        gen.Emit(OpCodes.Ldarg, i + 1);
        EmitBoxIfNeeded(gen, argumentType);
        gen.Emit(OpCodes.Stelem_Ref);
    }

    private static void EmitBoxIfNeeded(ILGenerator gen, Type argumentType)
    {
        if (argumentType.IsValueType)
        {
            gen.Emit(OpCodes.Box, argumentType);
        }
    }

    private Type FunctionReturnType()
    {
        return delegateType.GetGenericArguments().Last();
    }
}