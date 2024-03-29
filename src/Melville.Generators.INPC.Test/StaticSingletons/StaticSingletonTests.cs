﻿using System.Runtime.CompilerServices.ProductionGenerators.StaticSingletons;
using Melville.Generators.INPC.Test.UnitTests;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.StaticSingletons;

public class StaticSingletonTests
{
    private GeneratorTestBed RunTestWithDeclaredAttr(string s, string parentCode = "") =>
        new(new StaticSingletonGenerator(), $$"""
            namespace Outer
            {
                using Melville.INPC;
                {{s}}
            }
            """, typeof (StaticSingletonAttribute));

    [Fact]
    public void GenerateSingleton()
    {
        var tb = RunTestWithDeclaredAttr("[StaticSingleton] public partial class X {}");
        tb.LastFile().AssertContains("private X() {}");
        tb.LastFile().AssertContains("/// <summary>");
        tb.LastFile().AssertContains("/// Static singleton for Outer.X");
        tb.LastFile().AssertContains("/// </summary>");
        tb.LastFile().AssertContains("public static readonly Outer.X Instance = new();");
        tb.LastFile().AssertContains("#nullable enable");
    }
    [Fact]
    public void GenerateSingletonWithName()
    {
        var tb = RunTestWithDeclaredAttr("[StaticSingleton(\"FooBar\")] public partial class X {}");
        tb.LastFile().AssertContains("private X() {}"); 
        tb.LastFile().AssertContains("public static readonly Outer.X FooBar = new();");
    }
}