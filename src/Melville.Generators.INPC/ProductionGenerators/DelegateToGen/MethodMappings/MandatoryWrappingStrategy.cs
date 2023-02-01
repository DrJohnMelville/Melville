using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

public class MandatoryWrappingStrategy : IMethodWrappingStrategy
{
    private readonly string name;

    public MandatoryWrappingStrategy(string name)
    {
        this.name = name;
    }

    public virtual MappedMethod MapType(ITypeSymbol type)
    {
        return type.SpecialType == SpecialType.System_Void ?
            new MappedMethod(type, " {", $"; {name}();}}") :
            HasInputMapping(type);
    }

    protected MappedMethod HasInputMapping(ITypeSymbol type)
    {
        return new MappedMethod(type, $" => {name}(", ");");
    }
}