using System.ComponentModel.Design;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

public class MandatoryWrappingStrategy : IMethodWrappingStrategy
{
    private readonly string name;

    public MandatoryWrappingStrategy(string name)
    {
        this.name = name;
    }

    public MappedMethod MethodMappingFor(ITypeSymbol type) => 
        TryCreateConversionStrategy(type, MapType(type));

    protected virtual ITypeSymbol? MapType(ITypeSymbol inputType) => inputType;

    private MappedMethod TryCreateConversionStrategy(ITypeSymbol source, ITypeSymbol? destination) =>
        destination is null
            ? NoMethodMapping.Instance.MethodMappingFor(source)
            : TypeConversionStrategy(source, destination);

    private MappedMethod TypeConversionStrategy(ITypeSymbol source, ITypeSymbol destination) =>
        source.IsVoid()
            ? new MappedMethod(destination, " {", $"; {ExpandReturn(destination.IsVoid())}{name}();}}")
            : new MappedMethod(destination, $" => {name}(", ");");

    private string ExpandReturn(bool targetIsVoid) => targetIsVoid ? "" : "return ";
}