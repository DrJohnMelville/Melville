using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.SemanticUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.DependencyPropGen
{
    public static class ChangeFunctionFinder
    {
        public static string ComputerChangeFunction(RequestParser rp)
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
                    if (candidate.VerifyParameterTypes("System.Windows.DependencyObject", rp.TargetType()))
                        return $", (i,j)=>{rp.ParentType()}.{methodName}(i, ({rp.NullableTargetType()})(j.NewValue))";
                    if (candidate.VerifyParameterTypes(
                        "System.Windows.DependencyObject", rp.TargetType(), rp.TargetType()))
                        return $", (i,j)=>{rp.ParentType()}.{methodName}(i, ({rp.NullableTargetType()})(j.NewValue), ({rp.NullableTargetType()})(j.OldValue))";
                    return "";
                }
                // not static
                if (candidate.Parameters.Length == 0)
                    return $", (i,j)=>(({rp.ParentType()})i).{methodName}()";
                if (candidate.VerifyParameterTypes("System.Windows.DependencyPropertyChangedEventArgs"))
                    return $", (i,j)=>(({rp.ParentType()})i).{methodName}(j)";
                if (candidate.VerifyParameterTypes(rp.TargetType()))
                    return $", (i,j)=>(({rp.ParentType()})i).{methodName}(({rp.NullableTargetType()})(j.NewValue))";
                if (candidate.VerifyParameterTypes(rp.TargetType(), rp.TargetType()))
                    return $", (i,j)=>(({rp.ParentType()})i).{methodName}(({rp.NullableTargetType()})(j.NewValue), ({rp.NullableTargetType()})(j.OldValue))";

            }
            return "";
        }
    }
}