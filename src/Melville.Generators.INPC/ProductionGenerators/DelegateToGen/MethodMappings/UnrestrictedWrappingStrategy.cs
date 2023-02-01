using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

public class UnrestrictedWrappingStrategy : MandatoryWrappingStrategy
{
    private readonly SemanticModel semanticModel;
    private readonly IList<IMethodSymbol> valueMappers;
    private readonly ITypeSymbol? voidMapsTo;


    public UnrestrictedWrappingStrategy(string name, ITypeSymbol type, SemanticModel semanticModel) : base(name)
    {
        this.semanticModel = semanticModel;
        var sorter = new WrappingMethodSorter(type, name);
        valueMappers = sorter.NonVoidMap();
        voidMapsTo = sorter.VoidMap();
    }

    protected override ITypeSymbol? MapType(ITypeSymbol typeSymbol) =>
        typeSymbol.IsVoid() ? voidMapsTo : MapToConcreteType(typeSymbol);

    private ITypeSymbol? MapToConcreteType(ITypeSymbol typeSymbol) =>
        valueMappers
            .Select(i => TryConstructGenericMethod(i, typeSymbol))
            .FirstOrDefault(i => CanCallWithSingleArgument(i, typeSymbol))?.ReturnType;

    private IMethodSymbol? TryConstructGenericMethod(IMethodSymbol method, ITypeSymbol argumentType) =>
        method.IsGenericMethod ? method.Construct(argumentType) : method; 

    private bool CanCallWithSingleArgument(IMethodSymbol? method, ITypeSymbol argumentType) =>
        method is not null &&
        ArgumentMatchesParameter(argumentType, method.Parameters[0].Type);

    private bool ArgumentMatchesParameter(ITypeSymbol argumentType, ITypeSymbol parameterType) =>
        semanticModel.Compilation.HasImplicitConversion(argumentType, parameterType);
    
}