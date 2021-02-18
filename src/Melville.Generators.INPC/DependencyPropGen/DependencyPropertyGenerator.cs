using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Melville.Generators.INPC.PartialTypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.DependencyPropGen
{
    [Generator]
    public class DependencyPropertyGenerator: PartialTypeGenerator
    {
        public DependencyPropertyGenerator() : 
            base("DependencyPropertyGeneration",
                "Melville.DependencyPropertyGeneration.GenerateDPAttribute")
        {
        }

        protected override bool GlobalDeclarations(CodeWriter cw)
        {
            using (cw.ComplexAttribute("GenerateDP",
                AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Field |
                AttributeTargets.Event | AttributeTargets.Struct | AttributeTargets.Method, true))
            {
                cw.AppendLine("public bool Attached {get; set;}");
                cw.AppendLine("public bool Nullable {get; set;}");
                cw.AppendLine("public object? Default {get; set;}");
                cw.AppendLine("public GenerateDPAttribute(){}");
                cw.AppendLine("public GenerateDPAttribute(Type targetType, string propName=\"\"){}");
            }
            return true;
        }

        protected override bool 
            GenerateClassContents(IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, 
                CodeWriter cw)
        {
            var semanticModel = cw.Context.Compilation.GetSemanticModel(input.Key.SyntaxTree);
            var symbolInfo = semanticModel.GetDeclaredSymbol(input.Key);
            if (!(symbolInfo is ITypeSymbol classSymbol))
            {
                cw.ReportDiagnosticAt(input.Key, "DPGen0001", "Cannot find symbol info",
                    $"No symbol info for {input.Key.Identifier}", DiagnosticSeverity.Error);
                return false;
            }
            
            GenerateAttributes(input, cw, semanticModel, classSymbol);

            return true;
        }

        private static readonly SearchForAttribute searcher =
            new("Melville.DependencyPropertyGeneration.GenerateDPAttribute"); 
        private static void GenerateAttributes(
            IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, CodeWriter cw, 
            SemanticModel semanticModel, ITypeSymbol classSymbol)
        {
            foreach (var node in input)
            {
                foreach (var attribute in searcher.FindAllAttributes(node))
                {
                    GenerateSingleDependencyProperty(cw, attribute, semanticModel, classSymbol, node);
                }
            }
        }

        private static void GenerateSingleDependencyProperty(CodeWriter cw, AttributeSyntax attribute,
            SemanticModel semanticModel,
            ITypeSymbol classSymbol, MemberDeclarationSyntax targetMember)
        { 
            if (ParseAttribute(attribute, semanticModel, classSymbol, targetMember) is { } parser &&
                EnsureValidAttribute(cw, attribute, parser))
            {
                parser.Generate(cw);
            }
        }

        private static bool EnsureValidAttribute(CodeWriter cw, AttributeSyntax attribute, RequestParser parser)
        {
            if (parser.Valid()) return true;
            cw.ReportDiagnosticAt(attribute, "DPGen002", "Cannot resolve dependency property.",
                $"{attribute} does not have enough information to create DP.",
                DiagnosticSeverity.Error);
            return false;
        }

        private static RequestParser ParseAttribute(AttributeSyntax attribute, SemanticModel semanticModel,
            ITypeSymbol classSymbol, MemberDeclarationSyntax targetMember)
        {
            var parser = new RequestParser(semanticModel, classSymbol);
            parser.ParseAttributeTarget(targetMember);
            // we parse the target before the parameters, because the parameters override the conventions.
            parser.ParseAllParams(attribute.ArgumentList);
            return parser;
        }
    }
}