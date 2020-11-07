using Melville.Generators.INPC.Common.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.Common.AstUtilities
{
    public static class TypeSymbolOperations
    {
        public static void WriteTypeSymbolName(this CodeWriter writer, ITypeSymbol symbol)
        {
            writer.Append(symbol.ToString());
        }

        public static string FullyQualifiedName(this ITypeSymbol symbol)
        {
            return symbol.ToString();
        }
    }
}