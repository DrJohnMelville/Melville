using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.Macros;

[Generator]
public class MacroGenerator : IIncrementalGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.MacroCodeAttribute");

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(
                "Melville.INPC.MacroCodeAttribute", 
                (_,_) => true,
                (i, _) => i.TargetNode),
            Generate);
 
    }

    private void Generate(SourceProductionContext context, SyntaxNode node)
    {
        var cw = new SourceProductionCodeWriter(context);
        using (WriteCodeNear.Symbol(node, cw))
        {
            MacroSyntaxInterpreter.ExpandSingleMacroSet(node, cw);
        }
        cw.PublishCodeInFile(node, "MacroGen");
    }
}