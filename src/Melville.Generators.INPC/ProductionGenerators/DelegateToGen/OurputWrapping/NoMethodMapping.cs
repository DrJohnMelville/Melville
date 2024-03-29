﻿using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;

public class NoMethodMapping : IMethodWrappingStrategy
{
    public static readonly NoMethodMapping Instance = new();
    private NoMethodMapping() { }
    public MappedMethod MethodMappingFor(ITypeSymbol type)
    {
        return new(type, " => ", ";");
    }
}