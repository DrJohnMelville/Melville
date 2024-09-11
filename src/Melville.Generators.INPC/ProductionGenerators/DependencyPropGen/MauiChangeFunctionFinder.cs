using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;

public static class MauiChangeFunctionFinder
{
    public static string ComputeChangeFunction(RequestParser rp)
    {
        var methodName = $"On{rp.PropName}Changed";
        var candidates = rp.ParentSymbol.GetMembers(methodName);
        const string prefix = ", propertyChanged:";
        foreach (var candidate in candidates.OfType<IMethodSymbol>())
        {
            if (!candidate.ReturnsVoid) continue;
            if (candidate.IsStatic)
            {
                if (candidate.VerifyParameterTypes("Microsoft.Maui.Controls.BindableObject"))
                    return $"{prefix} (i,j,k)=>global::{rp.ParentType()}.{methodName}(i)";
                if (candidate.VerifyParameterTypes("Microsoft.Maui.Controls.BindableObject", rp.NullableTargetType()))
                    return $"{prefix} (i,j,k)=>global::{rp.ParentType()}.{methodName}(i,({rp.GlobalTargetType()})k)";
                if (candidate.VerifyParameterTypes("Microsoft.Maui.Controls.BindableObject", rp.NullableTargetType(), rp.NullableTargetType()))
                    return $"{prefix} (i,j,k)=>global::{rp.ParentType()}.{methodName}(i,({rp.GlobalTargetType()})j,({rp.GlobalTargetType()})k)";

                if (candidate.VerifyParameterTypes(rp.ParentType()))
                    return $"{prefix} (i,j,k)=>global::{rp.ParentType()}.{methodName}((global::{rp.ParentType()})i)";

                if (candidate.VerifyParameterTypes(rp.ParentType(), rp.NullableTargetType()))
                    return $"{prefix} (i,j,k)=>global::{rp.ParentType()}.{methodName}((global::{rp.ParentType()})i,({rp.GlobalTargetType()})k)";
                if (candidate.VerifyParameterTypes(rp.ParentType(), rp.NullableTargetType(), rp.NullableTargetType()))
                    return $"{prefix} (i,j,k)=>global::{rp.ParentType()}.{methodName}((global::{rp.ParentType()})i,({rp.GlobalTargetType()})j,({rp.GlobalTargetType()})k)";
                continue;
            }
            // not a static
            if (candidate.Parameters.Length == 0)
                return $"{prefix} (i,j,k)=>((global::{rp.ParentType()})i).{methodName}()";
            if (candidate.VerifyParameterTypes(rp.NullableTargetType()))
                return $"{prefix} (i,j,k)=>((global::{rp.ParentType()})i).{methodName}(({rp.GlobalTargetType()})k)";
            if (candidate.VerifyParameterTypes(rp.NullableTargetType(), rp.NullableTargetType()))
                return $"{prefix} (i,j,k)=>((global::{rp.ParentType()})i).{methodName}(({rp.GlobalTargetType()})j,({rp.GlobalTargetType()})k)";
        }
        return "";
    }
}