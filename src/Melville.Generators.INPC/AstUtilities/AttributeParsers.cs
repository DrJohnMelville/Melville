using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.AstUtilities
{
    public static class AttributeParsers
    {
        public static string ExtractArgumentFromAttribute(this AttributeArgumentSyntax arg)
        {
            return arg.Expression switch
            {
                LiteralExpressionSyntax les when les.Kind() ==SyntaxKind.StringLiteralExpression 
                    => les.Token.ValueText,
                _ => arg.ToString()
            };
        }

        public static IEnumerable<string> AttributeParameters(this AttributeSyntax attrib) =>
            attrib.ArgumentList?.Arguments.Select(AttributeParsers.ExtractArgumentFromAttribute)
            ?? Array.Empty<string>();
    }
}