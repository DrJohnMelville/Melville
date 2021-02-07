﻿using Melville.Generators.INPC.DependencyPropGen;
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
        public void GenerateNamedType(string codeName, string expandedName) =>
            MultiContentTest($"[GenerateDP(typeof({codeName}),\"Prop\")]",
                $"public static readonly System.Windows.DependencyProperty PropProperty =",
                "    System.Windows.DependencyProperty.Register(",
                $"    \"Prop\", typeof({expandedName}), typeof(Outer.C),",
                $"    new System.Windows.FrameworkPropertyMetadata(default({expandedName})));",
                $"public {expandedName} Prop",
                "{\r\n",
                $"    get => ({expandedName})this.GetValue(Outer.C.PropProperty);",
                $"    set => this.SetValue(Outer.C.PropProperty, value);",
                "}\r\n"
            );

        [Fact]
        public void NullableAttachedProperty() =>
            MultiContentTest("[GenerateDP(typeof(int), \"NullProp\", Attached=true, Nullable=true",
                "public static int? GetNullProp(System.Windows.DependencyObject obj)",
                "(int?)obj.GetValue(Outer.C.NullPropProperty)",
                "public static void SetNullProp(System.Windows.DependencyObject obj, int? value) =>");
        [Fact]
        public void NullableProperty() =>
            MultiContentTest("[GenerateDP(typeof(int), \"NullProp\", Nullable=true",
                "public int? NullProp",
                "get => (int?)this.GetValue(Outer.C.NullPropProperty);");

        [Theory]
        [InlineData("public static void OnPropChanged(System.Windows.DependencyObject obj, System.Windows.DependencyPropertyChangedEventArgs e) {}",
            "Outer.C.OnPropChanged")]
        [InlineData("public static void OnPropChanged(System.Windows.DependencyObject obj, int newVal) {}",
            "(i,j)=>Outer.C.OnPropChanged(i, (int)(j.NewValue))")]
        [InlineData("public static void OnPropChanged(System.Windows.DependencyObject obj, int newVal, int oldVal) {}",
            "(i,j)=>Outer.C.OnPropChanged(i, (int)(j.NewValue), (int)(j.OldValue))")]
        [InlineData("public void OnPropChanged(System.Windows.DependencyPropertyChangedEventArgs e) {}",
            "(i,j)=>((Outer.C)i).OnPropChanged(j)")]
         [InlineData("public void OnPropChanged() {}",
            "(i,j)=>((Outer.C)i).OnPropChanged()")]
         [InlineData("public void OnPropChanged(int newVal) {}",
            "(i,j)=>((Outer.C)i).OnPropChanged((int)(j.NewValue))")]
         [InlineData("public void OnPropChanged(int newVal, int oldValue) {}",
            "(i,j)=>((Outer.C)i).OnPropChanged((int)(j.NewValue), (int)(j.OldValue))")]
        public void CallOnChangedMethod(string methodDecl, string callSyntax)
        {
            MultiContentTest(methodDecl + "[GenerateDP(typeof(int), \"Prop\"]",
                $"    new System.Windows.FrameworkPropertyMetadata(default(int), {callSyntax}));"
                );
        }

        [Theory]
        [InlineData("[GenerateDPAttribute(typeof(int),\"Prop\", Attached=true)]")]
        [InlineData("[GenerateDP(typeof(int),\"Prop\", Attached=true)]")]
        [InlineData("[GenerateDP(name:\"Prop\", targetType:typeof(int), Attached=true)]")]
        
        public void GenerateExplicitProperty(string source) =>
            MultiContentTest(source,
                "public static readonly System.Windows.DependencyProperty PropProperty =",
                "    System.Windows.DependencyProperty.RegisterAttached(",
                "    \"Prop\", typeof(int), typeof(Outer.C),",
                "    new System.Windows.FrameworkPropertyMetadata(default(int)));",
                "public static int GetProp(System.Windows.DependencyObject obj) =>",
                "    (int)obj.GetValue(Outer.C.PropProperty);",
                "public static void SetProp(System.Windows.DependencyObject obj, int value) =>",
                "    obj.SetValue(Outer.C.PropProperty, value);"
            );

        private void MultiContentTest(string source, params string[] contents)
        {
            var res = RunTest(source);
            foreach (var content in contents)
            {
                res.FileContains("C.DependencyPropertyGeneration.cs", content);
            }
            
        }

    }
}