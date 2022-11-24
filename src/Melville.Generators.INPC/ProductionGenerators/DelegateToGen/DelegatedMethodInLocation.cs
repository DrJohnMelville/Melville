using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public record DelegatedMethodInLocation(SyntaxNode Target, IDelegatedMethodGenerator? Generator)
{
    public void GenerateIn(SourceProductionCodeWriter codeWriter)
    {
        if (Generator == null) return;
        using (codeWriter.GenerateInClassFile(Target,
                   "GeneratedDelegator"))
        {
            Generator.GenerateForwardingMethods(codeWriter);
        }
    }
}