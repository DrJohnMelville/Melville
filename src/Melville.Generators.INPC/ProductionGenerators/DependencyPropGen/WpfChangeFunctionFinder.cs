﻿using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;

public static class WpfChangeFunctionFinder
{
    public static string ComputeChangeFunction(RequestParser rp)
    {
        var methodName = $"On{rp.PropName}Changed";
        var candidates = rp.ParentSymbol.GetMembers(methodName);
        foreach (var candidate in candidates.OfType<IMethodSymbol>())
        {
            if (!candidate.ReturnsVoid) continue;
            if (candidate.IsStatic)
            {
                if (candidate.VerifyParameterTypes(
                        "System.Windows.DependencyObject", "System.Windows.DependencyPropertyChangedEventArgs"))
                    return $", {rp.ParentType()}.{methodName}";
                if (candidate.VerifyParameterTypes("System.Windows.DependencyObject", rp.NullableTargetType()))
                    return $", (i,j)=>{rp.ParentType()}.{methodName}(i, ({rp.NullableTargetType()})(j.NewValue))";
                if (candidate.VerifyParameterTypes(
                        "System.Windows.DependencyObject", rp.NullableTargetType(), rp.NullableTargetType()))
                    return $", (i,j)=>{rp.ParentType()}.{methodName}(i, ({rp.NullableTargetType()})(j.OldValue), ({rp.NullableTargetType()})(j.NewValue))";

                if (candidate.VerifyParameterTypes(
                        rp.ParentType(), "System.Windows.DependencyPropertyChangedEventArgs"))
                    return $", (i,j)=>{rp.ParentType()}.{methodName}((({rp.ParentType()})i),j)";
                if (candidate.VerifyParameterTypes(null, rp.NullableTargetType()))
                    return $", (i,j)=>{rp.ParentType()}.{methodName}((({candidate.Parameters[0].Type.FullyQualifiedName()})i), ({rp.NullableTargetType()})(j.NewValue))";
                if (candidate.VerifyParameterTypes(
                        null, rp.NullableTargetType(), rp.NullableTargetType()))
                    return $", (i,j)=>{rp.ParentType()}.{methodName}((({candidate.Parameters[0].Type.FullyQualifiedName()})i), ({rp.NullableTargetType()})(j.OldValue), ({rp.NullableTargetType()})(j.NewValue))";
                continue;
            }
            // not static
            if (candidate.Parameters.Length == 0)
                return $", (i,j)=>(({rp.ParentType()})i).{methodName}()";
            if (candidate.VerifyParameterTypes("System.Windows.DependencyPropertyChangedEventArgs"))
                return $", (i,j)=>(({rp.ParentType()})i).{methodName}(j)";
            if (candidate.VerifyParameterTypes(rp.NullableTargetType()))
                return $", (i,j)=>(({rp.ParentType()})i).{methodName}(({rp.NullableTargetType()})(j.NewValue))";
            if (candidate.VerifyParameterTypes(rp.NullableTargetType(), rp.NullableTargetType()))
                return $", (i,j)=>(({rp.ParentType()})i).{methodName}(({rp.NullableTargetType()})(j.OldValue), ({rp.NullableTargetType()})(j.NewValue))";

        }
        return "";
    }
}