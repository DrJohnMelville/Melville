using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Melville.Generators.INPC.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.DependencyPropGen
{
    public class RequestParser
    {
        private readonly SemanticModel semanticModel;
        public ITypeSymbol ParentSymbol { get; }
        public ITypeSymbol? Type { get; private set; }
        public string PropName { get; private set; } = "";
        public bool Attached { get; private set; }
        public bool Nullable { get; private set; }
        public string OnChanged { get; private set; } = "";

        public string TargetType() => Type?.FullyQualifiedName()??"";
        public string NullableTargetType() => (Type?.FullyQualifiedName()??"")+ (Nullable?"?":"");
        public string ParentType() => ParentSymbol.FullyQualifiedName();
        public string DependencyPropName() => PropName + "Property";
        public RequestParser(SemanticModel semanticModel, ITypeSymbol parentSymbol)
        {
            this.semanticModel = semanticModel;
            this.ParentSymbol = parentSymbol;
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
            switch (ne.Name.ToString())
            {
                case "Attached" :
                    Attached = ReadBoolLiteral(les);
                    break;
                case "Nullable" :
                    Nullable = ReadBoolLiteral(les);
                    break;
            }
        }

        private static bool ReadBoolLiteral(LiteralExpressionSyntax les)
        {
            return les.ToString().StartsWith("t");
        }

        public bool Valid()
        {
            return PropName.Length > 0 && Type != null;
        }
    }
}