﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class DelegationRequestParserFactory
{
    public static DelegationRequestParser Create(AttributeData attr, SemanticModel compilation)
    {
        var useExplicit = false;
        var WrapWith = "";
        var visibility = Accessibility.NotApplicable;
        foreach (var ca in attr.ConstructorArguments)
        {
            if (true.Equals(ca.Value)) useExplicit = true;
        }

        foreach (var na in attr.NamedArguments)
        {
            switch (na.Key)
            {
                case "WrapWith": WrapWith = na.Value.Value?.ToString()??""; break;
                case "ExplicitImplementation": useExplicit = Convert.ToBoolean(na.Value.Value); break;
                case "Visibility": visibility = (Accessibility)(na.Value.Value ?? Accessibility.NotApplicable); break;
            }
        }
        return new(useExplicit, WrapWith, compilation, visibility);
    }
}