using Melville.Generators.INPC.DependencyPropGen;
using Melville.Generators.INPC.INPC;
using Melville.Generators.INPC.Macros;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.DependencyPropGen
{
    public class PropGenUnitTest
    {
        private GeneratorTestBed RunTest(string s) =>
            new GeneratorTestBed(new DependencyPropertyGenerator(), @"
using Melville.DependencyPropertyGeneration;
using System;
namespace Outer
{
    public partial class C {" +
                                                       s +
                                                       @"
    private void Func();
}
}
");

        [Fact]
        public void CanUseProperty()
        {
            var res = RunTest("[GenerateDP(typeof(int),\"Prop\"]");
            res.FileContains("DependencyPropertyGenerationAttributes.DependencyPropertyGeneration.cs",
            "internal sealed class GenerateDPAttribute: Attribute");
        }

        [Theory]
        [InlineData("bool")]
        [InlineData("byte")]
        [InlineData("sbyte")]
        [InlineData("char")]
        [InlineData("decimal")]
        [InlineData("double")]
        [InlineData("float")]
        [InlineData("int")]
        [InlineData("uint")]
        [InlineData("long")]
        [InlineData("ulong")]
        [InlineData("short")]
        [InlineData("ushort")]
        [InlineData("object")]
        [InlineData("string")]
        public void GenerateBuiltInTypes(string builtinName) => GenerateNamedType(builtinName, builtinName);

        [Theory]
        [InlineData("String", "string")]
        [InlineData("System.String", "string")]
        [InlineData("Int32", "int")]
        [InlineData("C", "Outer.C")]
        public void GenerateNamedType(string codeName, string expandedName)
        {
            GenerateExplicitProperty($"[GenerateDP(typeof({codeName}),\"Prop\")]",
                $"//{expandedName}/Prop/False/");
        }

        [Theory]
        [InlineData("[GenerateDP(typeof(int),\"Prop\", Attached=true)]", "//int/Prop/True/")]
        [InlineData("[GenerateDP(typeof(int),\"Prop\", true)]", "//int/Prop/True/")]
        [InlineData("[GenerateDP(attached:true, name:\"Prop\", targetType:typeof(int))]",
            "//int/Prop/True/")]
        
        public void GenerateExplicitProperty(string source, string generated)
        {
            var res = RunTest(source);
            res.FileContains("C.DependencyPropertyGeneration.cs",
            generated);
        }

    }
}