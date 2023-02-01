using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

public class UnrestrictedWrappingStrategy : MandatoryWrappingStrategy
{
    private readonly SemanticModel semanticModel;
    private readonly IList<IMethodSymbol> candidateMethods;

    public UnrestrictedWrappingStrategy(string name, ITypeSymbol type, SemanticModel semanticModel) : base(name)
    {
        this.semanticModel = semanticModel;
        candidateMethods = CandidateMappingMethods(name, type);
    }

    private static List<IMethodSymbol> CandidateMappingMethods(string name, ITypeSymbol type)
    {
        return type.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(i => i.Name.Equals(name, StringComparison.Ordinal) &&
                        i.Parameters.Count(j => !j.IsOptional) < 2)
            .ToList();
    }

    public override MappedMethod MapType(ITypeSymbol type)
    {
        return (type, MapsToType(type)) switch
        {
            (_, null) => NoMethodMapping.Instance.MapType(type),
            ({SpecialType:not SpecialType.System_Void}, var mappedType) => base.MapType(mappedType),
            var (voidType, _) => HasInputMapping(voidType)
        };
    }

    protected virtual ITypeSymbol? MapsToType(ITypeSymbol typeSymbol)
    {
        return typeSymbol.SpecialType is SpecialType.System_Void
            ? MapToVoid(typeSymbol)
            : MapToConcreteType(typeSymbol);
    }

    private ITypeSymbol? MapToVoid(ITypeSymbol isVoid) =>
        candidateMethods.Any(i => !i.Parameters.Any(j => !j.IsOptional)) ? isVoid : null;

    private ITypeSymbol? MapToConcreteType(ITypeSymbol typeSymbol) =>
        candidateMethods
            .Select(i => TryConstructGenericMethod(i, typeSymbol))
            .OfType<IMethodSymbol>()
            .FirstOrDefault(i => CanCallWithSingleArgument(i, typeSymbol))?.ReturnType;

    private IMethodSymbol? TryConstructGenericMethod(IMethodSymbol method, ITypeSymbol argumentType) => method switch
    {
        { IsGenericMethod: false } => method,
        _ when FirstParameterIsGenericType(method) => method.Construct(argumentType),
        _ => null
    };

    private static bool FirstParameterIsGenericType(IMethodSymbol method) =>
        method.TypeParameters.Length == 1 &&
        method.Parameters.Length >= 1 &&
        SymbolEqualityComparer.Default.Equals(method.Parameters[0].Type, method.TypeParameters[0]);

    private bool CanCallWithSingleArgument(IMethodSymbol method, ITypeSymbol argumentType) =>
        method.Parameters.Length > 0 &&
        ArgumentMatchesParameter(argumentType, method.Parameters[0].Type) &&
        ExtraParametersAreOptional(method);

    private bool ArgumentMatchesParameter(ITypeSymbol argumentType, ITypeSymbol parameterType) =>
        semanticModel.Compilation.HasImplicitConversion(argumentType, parameterType);

    private static bool ExtraParametersAreOptional(IMethodSymbol method) =>
        method.Parameters.Skip(1).All(i => i.IsOptional);
}