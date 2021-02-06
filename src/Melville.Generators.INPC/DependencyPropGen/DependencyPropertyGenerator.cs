using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Melville.Generators.INPC.CodeWriters;
using Melville.Generators.INPC.PartialTypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.DependencyPropGen
{
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
                AttributeTargets.Event | AttributeTargets.Struct, true))
            {
                cw.AppendLine("public bool Attached {get; set;}");
                cw.AppendLine(
                    "public GenerateDP(Type targetType = typeof(void), string propName=\"\", bool attached = false){}");
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

        private static void GenerateAttributes(
            IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, CodeWriter cw, 
            SemanticModel semanticModel, ITypeSymbol classSymbol)
        {
            foreach (var node in input)
            {
                foreach (var attribute in node.AttributeLists.SelectMany(i => i.Attributes))
                {
                    GenerateSingleDependencyProperty(cw, attribute, semanticModel, classSymbol);
                }
            }
        }

        private static void GenerateSingleDependencyProperty(CodeWriter cw, AttributeSyntax attribute, SemanticModel semanticModel,
            ITypeSymbol classSymbol)
        {
            if (!attribute.Name.ToString().EndsWith("GenerateDP")) return;
            int pos = 0;
            var parser = new RequestParser(semanticModel, classSymbol);
            if (attribute.ArgumentList is { } al)
            {
                foreach (var argument in al.Arguments)
                {
                    parser.ParseParam(pos++, argument);
                }
            }

            parser.Generate(cw);
        }
    }
}