using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.SemanticUtilities
{
    public static class MethodSymbolOperations
    {
        public static bool VerifyParameterTypes(this IMethodSymbol ms, params string?[] parameters)
        {
            var paramSymbols = ms.Parameters;
            if (paramSymbols.Length != parameters.Length) return false;
            return paramSymbols
                .Zip(parameters, (i, j) => j == null || i.Type.FullyQualifiedName() == j)
                .All(i=>i);
        }
    }
}