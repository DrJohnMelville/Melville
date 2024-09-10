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
                    return $"{prefix} (i,j,k)=>global{rp.ParentType()}.{methodName}(i)";
                // note that we invert j and K here because maui puts the old value first but our
                // convention is to put the new value first because we often do not care about
                // the old value.
                if (candidate.VerifyParameterTypes("Microsoft.Maui.Controls.BindableObject", rp.TargetType()))
                    return $"{prefix} (i,j,k)=>global{rp.ParentType()}.{methodName}(i,(global{rp.TargetType()})K)";
                if (candidate.VerifyParameterTypes("Microsoft.Maui.Controls.BindableObject", rp.TargetType(), rp.TargetType()))
                    return $"{prefix} (i,j,k)=>global{rp.ParentType()}.{methodName}(i,(global{rp.TargetType()})K,(global{rp.TargetType()})j)";

                if (candidate.VerifyParameterTypes(rp.ParentType()))
                    return $"{prefix} (i,j,k)=>global{rp.ParentType()}.{methodName}((global::{rp.ParentType()})i)";
                // note that we invert j and K here because maui puts the old value first but our
                // convention is to put the new value first because we often do not care about
                // the old value.
                if (candidate.VerifyParameterTypes(rp.ParentType(), rp.TargetType()))
                    return $"{prefix} (i,j,k)=>global{rp.ParentType()}.{methodName}((global::{rp.ParentType()})i,(global{rp.TargetType()})K)";
                if (candidate.VerifyParameterTypes(rp.ParentType(), rp.TargetType(), rp.TargetType()))
                    return $"{prefix} (i,j,k)=>global{rp.ParentType()}.{methodName}((global::{rp.ParentType()})i,(global{rp.TargetType()})K,(global{rp.TargetType()})j)";
                continue;
            }
            // not a static
            if (candidate.Parameters.Length == 0)
                return $"{prefix} (i,j,k)=>((global::{rp.ParentType()})i).{methodName}()";
            if (candidate.VerifyParameterTypes(rp.TargetType()))
                return $"{prefix} (i,j,k)=>((global::{rp.ParentType()})i).{methodName}((global::{rp.TargetType()})k)";
            if (candidate.VerifyParameterTypes(rp.TargetType(), rp.TargetType()))
                return $"{prefix} (i,j,k)=>((global::{rp.ParentType()})i).{methodName}((global::{rp.TargetType()})k,(global::{rp.TargetType()})j)";
        }
        return "";
    }
}

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