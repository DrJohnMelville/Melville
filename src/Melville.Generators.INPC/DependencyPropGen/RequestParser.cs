using System;
using System.Runtime.InteropServices.ComTypes;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.DependencyPropGen
{
    public class RequestParser
    {
        private readonly SemanticModel semanticModel;
        private readonly ITypeSymbol parentSymbol;
        public ITypeSymbol? Type { get; private set; }
        public string PropName { get; private set; } = "";
        public bool Attached { get; private set; }
        public string OnChanged { get; private set; } = "";

        public RequestParser(SemanticModel semanticModel, ITypeSymbol parentSymbol)
        {
            this.semanticModel = semanticModel;
            this.parentSymbol = parentSymbol;
        }

        public void ParseParam(int position, AttributeArgumentSyntax syntax)
        {
            switch (position, syntax.NameEquals, syntax.NameColon, syntax.Expression) // remember to handle nameequals and colonequals
            {
                case (_, {} ne, _, LiteralExpressionSyntax les):
                    ParseNamedProperty(ne, les);
                    break;
                case (_, _, {} nc, var expr):
                    ParseColonParameter(nc.Name.ToString(), syntax);
                    break;
                case (0, _, _,TypeOfExpressionSyntax tos):
                    ParseTargetType(tos);
                    break;
                case (1, _, _, LiteralExpressionSyntax):
                    ParsePropName(syntax);
                    break;
                case (2, _, _, LiteralExpressionSyntax les):
                    ParseAttachedProperty(les);
                    break;
            }
        }

        private void ParseColonParameter(string name, AttributeArgumentSyntax expr)
        {
            switch (name, expr.Expression)
            {
                case ("targetType", TypeOfExpressionSyntax tos):
                    ParseTargetType(tos);
                    break;
                case ("name", _):
                    ParsePropName(expr);
                    break;
                case ("attached", LiteralExpressionSyntax les):
                    ParseAttachedProperty(les);
                    break;
            }
        }

        private void ParseTargetType(TypeOfExpressionSyntax tos)
        {
            Type = tos.ToTypeSymbol(semanticModel);
        }

        private void ParsePropName(AttributeArgumentSyntax syntax)
        {
            PropName = syntax.ExtractArgumentFromAttribute();
        }

        private void ParseNamedProperty(NameEqualsSyntax ne, LiteralExpressionSyntax les)
        {
            if (ne.Name.ToString() == "Attached")
            {
                ParseAttachedProperty(les);
            }
        }

        private void ParseAttachedProperty(LiteralExpressionSyntax les) =>
            Attached = les.ToString().StartsWith("t");

        public void Generate(CodeWriter cw)
        {
            cw.AppendLine($"//{Type}/{PropName}/{Attached}/{OnChanged}");
        }
    }
}