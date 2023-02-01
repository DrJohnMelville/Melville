using System.ComponentModel.Design;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

public class MandatoryWrappingStrategy : IMethodWrappingStrategy
{
    private readonly string name;

    public MandatoryWrappingStrategy(string name)
    {
        this.name = name;
    }

    public MappedMethod MethodMappingFor(ITypeSymbol type) => TypeConversionStrategy(type, MapType(type));

    protected virtual ITypeSymbol MapType(ITypeSymbol inputType) => inputType;

    private MappedMethod TypeConversionStrategy(ITypeSymbol source, ITypeSymbol destination) =>
        IsVoid(source)
            ? new MappedMethod(destination, " {", $"; {ExpandReturn(IsVoid(destination))}{name}();}}")
            : new MappedMethod(destination, $" => {name}(", ");");

    private string ExpandReturn(bool targetIsVoid) => targetIsVoid ? "" : "return ";

    private static bool IsVoid(ITypeSymbol type) => type.SpecialType == SpecialType.System_Void;
}