using System.Linq;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

[Generator]

#if true
public class INPCGenerator : ClassWithLabeledMembersGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.AutoNotifyAttribute");

    public INPCGenerator() : base(attrFinder)
    {
    }
    protected override ILabeledMembersSyntaxModel CreateMemberRecord(TypeDeclarationSyntax targetTypeDecl) => 
        new InpcSyntaxModel(targetTypeDecl);
}
#else
public class INPCGenerator: IIncrementalGenerator
{
    private const string attributeFullName = "Melville.INPC.AutoNotifyAttribute";
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(attributeFullName,
                (i,_)=> i is TypeDeclarationSyntax or VariableDeclaratorSyntax,
                (i,_)=> i)
                .Collect()
                .SelectMany((i,_) => i.GroupBy(j=>j.TargetSymbol.ThisOrContainingTypeSymbol()))
                .Select((j,_)=>j),
            Generate
        );
    }

    private void Generate(SourceProductionContext arg1, IGrouping<ITypeSymbol, GeneratorAttributeSyntaxContext> arg2)
    {
        ;
    }
}
#endif
