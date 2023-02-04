using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;

public class UnrestrictedWrappingStrategy : MandatoryWrappingStrategy
{
    public SemanticModel SemanticModel { get; }
    private readonly IList<IMethodSymbol> valueMappers;
    private readonly ITypeSymbol? voidMapsTo;


    public UnrestrictedWrappingStrategy(string name, ITypeSymbol type, SemanticModel semanticModel) : base(name)
    {
        this.SemanticModel = semanticModel;
        var sorter = new WrappingMethodSorter(type, name);
        valueMappers = sorter.NonVoidMap();
        voidMapsTo = sorter.VoidMap();
    }

    protected override ITypeSymbol? MapType(ITypeSymbol typeSymbol) =>
        typeSymbol.IsVoid() ? voidMapsTo : MapToConcreteType(typeSymbol);

    private ITypeSymbol? MapToConcreteType(ITypeSymbol typeSymbol) =>
        valueMappers
            .Select(i => TryConstructGenericMethod(i, typeSymbol))
            .FirstOrDefault(i => FirstArgumentMatches(i, typeSymbol))?.ReturnType;

    private IMethodSymbol TryConstructGenericMethod(IMethodSymbol method, ITypeSymbol argumentType) =>
        method.IsGenericMethod ? method.Construct(argumentType) : method; 

    private bool FirstArgumentMatches(IMethodSymbol method, ITypeSymbol argumentType) =>
        IsAssignableTo(argumentType, method.Parameters[0].Type);

    protected bool IsAssignableTo(ITypeSymbol? argumentType, ITypeSymbol? parameterType) =>
        SemanticModel.Compilation.HasImplicitConversion(argumentType, parameterType);
}