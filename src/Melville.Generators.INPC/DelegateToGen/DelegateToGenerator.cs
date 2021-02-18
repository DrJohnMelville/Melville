using System;
using System.Linq;
using Melville.Generators.INPC.CodeWriters;
using Melville.Generators.INPC.PartialTypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.DelegateToGen
{
    public class DelegateToGenerator: PartialTypeGenerator<DelegationRequest>
    {
        public DelegateToGenerator() : 
            base("DelegateToGeneration", "Melville.DelegateToGeneration.DelegateToAttribute")
        {
        }

        protected override string ClassSuffix(DelegationRequest input) => input.ClassSuffix();

        protected override DelegationRequest PreProcess(
            IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, 
            GeneratorExecutionContext context) =>
            DelegationRequestParser.Parse(GetSemanticModel(input, context), input.Key, input);

        private static SemanticModel GetSemanticModel(
            IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, 
            GeneratorExecutionContext context) => 
            context.Compilation.GetSemanticModel(input.Key.SyntaxTree);

        protected override bool GlobalDeclarations(CodeWriter cw)
        {
            using var block = cw.ComplexAttribute("DelegateTo",
                AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method);
            cw.AppendLine("public DelegateToAttribute(bool explicitImplementation = false){}");
            return true;
        }

        protected override bool GenerateClassContents(DelegationRequest input, CodeWriter cw)
        {
            input.GenerateForwardingMethods(cw);
            return true;
        }
    }
}