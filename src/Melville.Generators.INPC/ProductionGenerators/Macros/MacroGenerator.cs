using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.Macros;

[Generator]
public class MacroGenerator : IIncrementalGenerator
{
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
        using (cw.GenerateInClassFile(node, "MacroGen"))
        {
            MacroSyntaxInterpreter.ExpandSingleMacroSet(node, cw);
        }
    }
}