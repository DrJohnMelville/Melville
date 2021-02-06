using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.AstUtilities
{
    public static class TypeOfParsers
    {
        public static ITypeSymbol? ToTypeSymbol(this TypeOfExpressionSyntax tos, SemanticModel semanticModel)
            => tos.ToTypeSymbol(tos.Type.ToString(), semanticModel);

        public static ITypeSymbol? ToTypeSymbol(this SyntaxNode location, string typeName, SemanticModel semanticModel)
        {
            return WellKnownTypes(typeName, semanticModel.Compilation) ??
                   SearchForLocalTypeName(location, typeName, semanticModel) ??
                   semanticModel.Compilation.GetTypeByMetadataName(typeName);
        }
        
        private static ITypeSymbol? WellKnownTypes(string name, Compilation compilation) => name switch
        {
            "bool" => compilation.GetSpecialType(SpecialType.System_Boolean),
            "byte" => compilation.GetSpecialType(SpecialType.System_Byte),
            "sbyte" => compilation.GetSpecialType(SpecialType.System_SByte),
            "char" => compilation.GetSpecialType(SpecialType.System_Char),
            "decimal" => compilation.GetSpecialType(SpecialType.System_Decimal),
            "double" => compilation.GetSpecialType(SpecialType.System_Double),
            "float" => compilation.GetSpecialType(SpecialType.System_Single),
            "int" => compilation.GetSpecialType(SpecialType.System_Int32),
            "uint" => compilation.GetSpecialType(SpecialType.System_UInt32),
            "long" => compilation.GetSpecialType(SpecialType.System_Int64),
            "ulong" => compilation.GetSpecialType(SpecialType.System_UInt64),
            "ushort" => compilation.GetSpecialType(SpecialType.System_UInt16),
            "short" => compilation.GetSpecialType(SpecialType.System_Int16),
            "object" => compilation.GetSpecialType(SpecialType.System_Object),
            "string" => compilation.GetSpecialType(SpecialType.System_String),
            _ => null
        };

        private static ITypeSymbol? SearchForLocalTypeName(
            SyntaxNode tos, string name, SemanticModel semanticModel) =>
            semanticModel.LookupNamespacesAndTypes(tos.Span.Start, null, name)
                .OfType<ITypeSymbol>()
                .FirstOrDefault();

    }
}