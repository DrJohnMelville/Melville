using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.AstUtilities;

public static class TypeOfParsers
{
    public static ITypeSymbol? ToTypeSymbol(this TypeOfExpressionSyntax tos, SemanticModel semanticModel)
        => semanticModel.GetSymbolInfo(tos.Type).Symbol as ITypeSymbol;
}