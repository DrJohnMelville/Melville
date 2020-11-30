using Melville.Generators.Tools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.Tools.AstUtilities
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