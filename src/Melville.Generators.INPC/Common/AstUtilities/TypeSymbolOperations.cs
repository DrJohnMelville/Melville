using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Melville.Generators.INPC.Common.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.Common.AstUtilities
{
    public static class TypeSymbolOperations
    {
        public static void WriteTypeSymbolName(this CodeWriter writer, ITypeSymbol symbol)
        {
            AppendSymbolName(symbol, writer.Append);
        }

        public static string FullyQualifiedName(this ITypeSymbol symbol)
        {
            var sb = new StringBuilder();
            AppendSymbolName(symbol, s=>sb.Append(s));
            return sb.ToString();
        }

        private static void AppendSymbolName(ITypeSymbol symbol,
            Action<string> appender)
        {
            appender(symbol.ToString());
            // WriteContainingSymbolNames(appender, symbol);
            // appender(symbol.Name);
            // AppendTypeSuffixes(symbol as INamedTypeSymbol, appender);
        }

        private static void AppendTypeSuffixes(INamedTypeSymbol? symbol, Action<string> appender)
        {
            if (symbol == null) return;
            TryAppendTypeArgumentList(symbol.TypeArguments, appender);
            TryAppendNullableMark(symbol, appender);
        }

        private static void TryAppendNullableMark(INamedTypeSymbol symbol, Action<string> appender)
        {
            if (symbol.NullableAnnotation == NullableAnnotation.Annotated)
            {
                appender("?");
            }
        }

        private static void TryAppendTypeArgumentList(
            ImmutableArray<ITypeSymbol> arguments, Action<string> appender)
        {
            if (arguments.Length == 0) return;
            appender("<");
            CommaSeparatedSymbolList(arguments, appender);
            appender(">");
        }

        private static void CommaSeparatedSymbolList(ImmutableArray<ITypeSymbol> arguments, Action<string> appender)
        {
            AppendSymbolName(arguments[0], appender);
            foreach (var argument in arguments.Skip(1))
            {
                appender(",");
                AppendSymbolName(argument, appender);
            }
        }

        private static void WriteContainingSymbolNames(Action<string> appender, ITypeSymbol symbol)
        {
            foreach (var parentSymbol in ContainersOf(symbol).Reverse())
            {
                appender(parentSymbol.Name);
                appender(ParentSymbolTerminator(parentSymbol));
            }
        }

        private static string ParentSymbolTerminator(ISymbol parentSymbol) =>
            parentSymbol switch
            {
                ITypeSymbol type => "+",
                _=> "."
            };

        private static IEnumerable<ISymbol> ContainersOf(ITypeSymbol symbol)
        {
            var ret = symbol.ContainingSymbol;
            while (!(ret is INamespaceSymbol ns && ns.IsGlobalNamespace))
            {
                yield return ret;
                ret = ret.ContainingSymbol;
            }
        }
    }
}