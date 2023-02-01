using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

public interface IMethodWrappingStrategy
{
    public MappedMethod MapType(ITypeSymbol type);
}