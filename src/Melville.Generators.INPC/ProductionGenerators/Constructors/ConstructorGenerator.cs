using System.Linq;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.ProductionGenerators.Constructors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.Constructors;

#if false
[Generator]
public class ConstructorGenerator : ClassWithLabeledMembersGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.FromConstructorAttribute");

    public ConstructorGenerator() : base(attrFinder)
    {
    }

    protected override ILabeledMembersSyntaxModel CreateMemberRecord(TypeDeclarationSyntax targetTypeDecl) =>
        new ConstructorClassSyntaxModel(targetTypeDecl);
}
#else
[Generator]
public class ConstructorGenerator : IIncrementalGenerator
{
    public const string AttributeName = "Melville.INPC.FromConstructorAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(AttributeName,
                    static (i, _) => i is
                        ClassDeclarationSyntax or VariableDeclaratorSyntax or PropertyDeclarationSyntax,
                    static (i, _) => i)
                .Collect()
                .SelectMany(static (i, _) => i
                .GroupBy(KeySelector, SymbolEqualityComparer.Default)
                .Select(static i=>i.Key)),
            Generate
        );
    }

    private void Generate(SourceProductionContext codeTarget, ISymbol classToGenerate)
    {
        if (classToGenerate is not ITypeSymbol typeToGenerate) return;
        var cw = new SourceProductionCodeWriter(codeTarget);
        using (cw.GenerateInClassFile(classToGenerate, "Constructors"))
        {
            new ConstructorGenerator2(typeToGenerate, cw).Genarate();
        }
    }

    private static ITypeSymbol KeySelector(GeneratorAttributeSyntaxContext j)
    {
        return j.TargetSymbol is ITypeSymbol sym ? sym : j.TargetSymbol.ContainingType;
    }
 
}

#endif