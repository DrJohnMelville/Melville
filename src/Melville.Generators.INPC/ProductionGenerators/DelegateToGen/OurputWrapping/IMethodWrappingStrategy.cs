using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;

public interface IMethodWrappingStrategy
{
    public MappedMethod MethodMappingFor(ITypeSymbol type);
}