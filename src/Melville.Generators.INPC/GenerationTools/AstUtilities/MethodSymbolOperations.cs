using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.AstUtilities;

public static class MethodSymbolOperations
{
    public static bool VerifyParameterTypes(this IMethodSymbol ms, params string?[] parameters)
    {
        var paramSymbols = ms.Parameters;
        if (paramSymbols.Length != parameters.Length) return false;
        return paramSymbols
            .Zip(parameters, (i, j) => j == null || TypeSymbolOperations.FullyQualifiedName(i.Type) == j)
            .All(i=>i);
    }
}