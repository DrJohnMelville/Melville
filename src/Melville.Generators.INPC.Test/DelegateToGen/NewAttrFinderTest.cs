using System;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen;
[Obsolete]
public class NewAttrFinderTest
{
    private GeneratorTestBed RunTest() => new(new TestGenerator(), $$"""
    
        namespace Melville.INPC {
            public class HasAttrAttribute:Attribute{}         
        }
        namespace Outer
        {
            using Melville.INPC;
            
            public partial class C: {
              [HasAttr] public void Foo() {}
              public void Bar()
            }
        }
        """);

    [Fact]
    public void FindAttr()
    {
        var tb = RunTest();
        tb.FileContains("out.cs", "//[HasAttr] public void Foo() {}");
    }
}