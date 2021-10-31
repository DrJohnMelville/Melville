using System.Linq;
using Melville.Generators.INPC.CodeWriters;
using Melville.Generators.INPC.PartialTypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.DelegateToGen;

[Generator]
public class DelegateToGenerator: PartialTypeGenerator<DelegationRequest>
{
    public DelegateToGenerator() : 
        base("DelegateToGeneration", "Melville.INPC.DelegateToAttribute")
    {
    }
        
    protected override string ClassSuffix(DelegationRequest input) => "";

    protected override DelegationRequest PreProcess(
        IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, 
        GeneratorExecutionContext context) =>
        DelegationRequestParser.Parse(GetSemanticModel(input, context), input.Key, input);

    private static SemanticModel GetSemanticModel(
        IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, 
        GeneratorExecutionContext context) => 
        context.Compilation.GetSemanticModel(input.Key.SyntaxTree);

    protected override bool GlobalDeclarations(CodeWriter cw) => true;

    protected override bool GenerateClassContents(DelegationRequest input, CodeWriter cw)
    {
        input.GenerateForwardingMethods(cw);
        return true;
    }
}