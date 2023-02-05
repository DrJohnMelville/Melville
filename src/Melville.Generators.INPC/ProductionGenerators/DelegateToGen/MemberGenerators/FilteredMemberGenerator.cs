using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators;

public class FilteredMemberGenerator : IMemberGenerator
{
    public static FilteredMemberGenerator Instance = new();
    private FilteredMemberGenerator() { }
    public bool IsSuppressedBy(ISymbol comparisonItem) => true;
        
    public void WriteSymbol(CodeWriter cw)
    {
    }
}