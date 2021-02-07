using System;
using System.Collections;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
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
            ParentSymbol = parentSymbol;
        }

        public void ParseAllParams(AttributeArgumentListSyntax? arguments)
        {
            if (arguments?.Arguments is not { } args) return;
            var pos = 0;
            foreach (var argument in args)
            {
                ParseParam(pos++, argument);
            }
        }

        private void ParseParam(int position, AttributeArgumentSyntax syntax)
        {
            switch (position, syntax.NameEquals, syntax.NameColon, syntax.Expression) // remember to handle nameequals and colonequals
            {
                case (_, {} ne, _, LiteralExpressionSyntax les):
                    ParseNamedProperty(ne, les);
                    break;
                case (_, _, {} nc, _):
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

        private void ParseTargetType(TypeOfExpressionSyntax tos) => 
            Type = tos.ToTypeSymbol(semanticModel);

        private void ParsePropName(AttributeArgumentSyntax syntax) => 
            PropName = syntax.ExtractArgumentFromAttribute();

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

        private static bool ReadBoolLiteral(LiteralExpressionSyntax les) => 
            les.ToString().StartsWith("t");

        public bool Valid() => PropName.Length > 0 && Type != null;

        public void ParseAttributeTarget(MemberDeclarationSyntax targetMember)
        {
            if (TryParseModifyMethod(targetMember)) return;
        }

        private bool ComputeAttached(bool isStatic, ImmutableArray<IParameterSymbol> symbolParameters) =>
            isStatic && symbolParameters.Length > 0 &&
            symbolParameters[0].Type.FullyQualifiedName() == "System.Windows.DependencyObject";

        private bool TryParseModifyMethod(MemberDeclarationSyntax targetMember)
        {
            if (targetMember is not MethodDeclarationSyntax mds) return false;
            if (!TryParseModifyMethodName(mds.Identifier.ToString(), out var extractedName)) return false;
            PropName = extractedName;
            if (semanticModel.GetDeclaredSymbol(targetMember) is IMethodSymbol symbol)
            {
                Attached = ComputeAttached(symbol.IsStatic, symbol.Parameters);
                InferTargetType(symbol);
            }
            return true;
        }

        private void InferTargetType(IMethodSymbol symbol) => 
            Type = FilterTypeSymbolForNullability(symbol.Parameters.LastOrDefault()?.Type);

        private ITypeSymbol? FilterTypeSymbolForNullability(ITypeSymbol? sym)
        {
            if ( sym == null || !IsNullableReferenceType(sym)) return sym;
            Nullable = true;
            return sym.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
        }
        private static bool IsNullableReferenceType(ITypeSymbol rawType) =>
            rawType.IsReferenceType && 
            rawType.NullableAnnotation == NullableAnnotation.Annotated;

        private static Regex ModifyMethodNameParser = new (@"^On(\w+)Changed$");
        private bool TryParseModifyMethodName(string name, out string methName)
        {
            var match = ModifyMethodNameParser.Match(name);
            methName = match.Success ? match.Groups[1].Value : "";
            return match.Success;
        }
    }
}