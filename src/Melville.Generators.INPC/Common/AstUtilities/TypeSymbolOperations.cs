using System;
using System.Collections.Generic;
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
            WriteContainingSymbolNames(appender, symbol);
            appender(symbol.Name);
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