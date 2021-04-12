using System;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.AstUtilities
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

        public static bool HasMethod(
            this ITypeSymbol? symbol, Type? returnType, string name, params Type[] parameters)
        {
            if (symbol == null) return false;
            return HasLocalMethod(symbol, returnType, name, parameters) ||
                   HasMethod(symbol.BaseType, returnType, name, parameters);
        }

        public static bool HasLocalMethod(
            this ITypeSymbol symbol, Type? returnType, string name, params Type[] parameters) =>
            symbol.GetMembers().OfType<IMethodSymbol>().Any(m =>
                MethodMatches(returnType, name, parameters, m));

        private static bool MethodMatches(Type? returnType, string name, Type[] parameters, IMethodSymbol m)
        {
            return ReturnTypeMatches(returnType, m) &&
                   name.Equals(m.Name, StringComparison.Ordinal) &&
                   ParametersMatch(parameters, m.Parameters);
        }

        private static bool ParametersMatch(Type[] expected, ImmutableArray<IParameterSymbol> found) =>
            (expected.Length == found.Length)
            && expected.Zip(found, (i, j) => TypeMatches(j.Type, i)).All(i=>i);

        private static bool ReturnTypeMatches(Type? returnType, IMethodSymbol m) =>
            returnType == null ? m.ReturnsVoid :
                TypeMatches(m.ReturnType, returnType);

        public static bool TypeMatches(ITypeSymbol symbol, Type type) => 
            symbol.FullyQualifiedName().Equals(type.FullyQualifiedName());
    }
}