﻿using Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;
using Melville.Generators.INPC.Test.UnitTests;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.DependencyPropGen;

public class PropGenUnitTest
{
    private GeneratorTestBed RunTest(string s) =>
        new GeneratorTestBed(new DependencyPropertyGenerator(), $$"""
            namespace Outer
            {
                using Melville.INPC;
                using System;
                public partial class C 
                {
                    {{s}}
                    private void Func();
                }
            }
            """, typeof(GenerateDPAttribute));

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
    [InlineData("System.Collections.Generic.List<string>", "System.Collections.Generic.List<string>")]
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
            "}\r\n",
            $"/// DependencyProperty field for Prop",
            $"/// DependencyProperty property for Prop"
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
            "get => (int?)this.GetValue(Outer.C.NullPropProperty);",
            "\"NullProp\", typeof(int?)",
            "default(int?)");
    [Fact]
    public void NullableReferenceProperty() =>
        MultiContentTest("[GenerateDP(typeof(string), \"NullProp\", Nullable=true",
            "public string? NullProp",
            "get => (string?)this.GetValue(Outer.C.NullPropProperty);",
            "\"NullProp\", typeof(string)",
            "default(string?)");

    [Theory]
    [InlineData("public static void OnPropChanged(C obj, System.Windows.DependencyPropertyChangedEventArgs e) {}",
        "(i,j)=>Outer.C.OnPropChanged(((Outer.C)i),j)")]
    [InlineData("public static void OnPropChanged(C obj, int newVal) {}",
        "(i,j)=>Outer.C.OnPropChanged(((Outer.C)i), (int)(j.NewValue))")]
    [InlineData("public static void OnPropChanged(C obj, int oldVal, int newVal) {}",
        "(i,j)=>Outer.C.OnPropChanged(((Outer.C)i), (int)(j.OldValue), (int)(j.NewValue))")]

    [InlineData("public static void OnPropChanged(System.Windows.DependencyObject obj, System.Windows.DependencyPropertyChangedEventArgs e) {}",
        "Outer.C.OnPropChanged")]
    [InlineData("public static void OnPropChanged(System.Windows.DependencyObject obj, int newVal) {}",
        "(i,j)=>Outer.C.OnPropChanged(i, (int)(j.NewValue))")]
    [InlineData("public static void OnPropChanged(System.Windows.DependencyObject obj, int oldVal, int newVal) {}",
        "(i,j)=>Outer.C.OnPropChanged(i, (int)(j.OldValue), (int)(j.NewValue))")]
    [InlineData("public static void OnPropChanged(string obj, int oldVal, int newVal) {}",
        "(i,j)=>Outer.C.OnPropChanged(((string)i), (int)(j.OldValue), (int)(j.NewValue))")]
    [InlineData("public static void OnPropChanged(string obj, int newVal) {}",
        "(i,j)=>Outer.C.OnPropChanged(((string)i), (int)(j.NewValue))")]
    [InlineData("public void OnPropChanged(System.Windows.DependencyPropertyChangedEventArgs e) {}",
        "(i,j)=>((Outer.C)i).OnPropChanged(j)")]
    [InlineData("public void OnPropChanged() {}",
        "(i,j)=>((Outer.C)i).OnPropChanged()")]
    [InlineData("public void OnPropChanged(int newVal) {}",
        "(i,j)=>((Outer.C)i).OnPropChanged((int)(j.NewValue))")]
    [InlineData("public void OnPropChanged(int oldVal, int newValue) {}",
        "(i,j)=>((Outer.C)i).OnPropChanged((int)(j.OldValue), (int)(j.NewValue))")]
    public void CallOnChangedMethod(string methodDecl, string callSyntax)
    {
        MultiContentTest(methodDecl + "[GenerateDP(typeof(int), \"Prop\"]",
            $"    new System.Windows.FrameworkPropertyMetadata(default(int), {callSyntax}));"
        );
    }

    [Theory]
    [InlineData(
        "[GenerateDP(typeof(int))] public static void OnPropertyChanged(System.Windows.DependencyObject obj, System.Windows.DependencyPropertyChangedEventArgs e) {}",
        true)]
    [InlineData(
        "[GenerateDP] public static void OnPropertyChanged(System.Windows.DependencyObject obj, int newValue) {}",
        true)]
    [InlineData(
        "[GenerateDP(typeof(int))] public static void OnPropertyChanged(C obj, System.Windows.DependencyPropertyChangedEventArgs e) {}",
        false)]
    [InlineData("[GenerateDP] public static void OnPropertyChanged(C obj, int newValue) {}",false)]
    [InlineData("[GenerateDP] public static void OnPropertyChanged(C obj, int newValue, int oldValue) {}", false)]
    [InlineData("[GenerateDP] public void OnPropertyChanged(int newValue) {}",false)]
    [InlineData("[GenerateDP] public void OnPropertyChanged(int newValue, int oldValue) {}", false)]
    public void InferFromModifyMethod(string code, bool attached)
    {
        MultiContentTest(code, 
            $"    \"Property\", typeof(int), typeof(Outer.C),",
            attached?"RegisterAttached(":".Register("
        );
    }

    [Theory]
    [InlineData("int", "int")]
    [InlineData("int?", "int?")]
    [InlineData("string", "string")]
    [InlineData("string?", "string")]
    [InlineData("object", "object")]
    [InlineData("object?", "object")]
    [InlineData("System.Collections.Generic.List<string>", "System.Collections.Generic.List<string>")]
    [InlineData("System.Collections.Generic.List<string>?", "System.Collections.Generic.List<string>")]
    public void InferNullable(string type, string storageType)
    {
        MultiContentTest($"[GenerateDP] public void OnPropertyChanged({type} newValue) {{}}",
            $"public {type} Property",
            $"typeof({storageType})");
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
            "    obj.SetValue(Outer.C.PropProperty, value);",
            "// DependencyProperty getter for Prop",
            "// DependencyProperty setter for Prop"
        );

    [Fact]
    public void GenerateFromDeclaration()
    {
        var res = RunTest(@"
              [GenerateDP] public static readonly System.Windows.DependencyProperty NovelProperty =
                System.Windows.DependencyProperty.Register(""Novel"", typeof(int),
                typeof(c), new System.Windows.FrameworkPropertyMetadata(0));");
            
        res.LastFile().AssertDoesNotContain(".Register");
        res.LastFile().AssertContains("get => (int)this.GetValue(Outer.C.NovelProperty);");
        res.LastFile().AssertContains("set => this.SetValue(Outer.C.NovelProperty, value);");
    }

    [Fact]
    public void GenerateExplicitDocumentation()
    {
        var res = RunTest("""
        [GenerateDP(typeof(int), "X", XmlDocumentation="x\r\ny")] int y;
        """);
        res.LastFile().AssertContains("/// x");
        res.LastFile().AssertContains("/// y");
    }

    [Theory]
    [InlineData("[GenerateDP(typeof(int),\"Prop\", Default=10)]", "Metadata(10)")]
    [InlineData("[GenerateDP(typeof(int),\"Prop\", Default=\"@10\")]", "Metadata(10)")]
    [InlineData("[GenerateDP(typeof(int),\"Prop\", Default=-10)]", "Metadata(-10)")]
    [InlineData("[GenerateDP] private void OnPropChanged(int i = 10)", "Metadata(10,")]
    [InlineData("[GenerateDP] private void OnPropChanged(string i = \"Hello\")", "Metadata(\"Hello\",")]
    [InlineData("[GenerateDP(typeof(string),\"Prop\", Default=\"Hello\")]", "Metadata(\"Hello\")")]
    public void GeneratePropWithDefault(string prompt, string defaultText)
    {
        var res = RunTest(prompt);
        res.LastFile().AssertContains(defaultText);
            
    }
    [Theory]
    [InlineData("[GenerateDP][field:FieldProp][property:PropProp] private void OnPropChanged(int new){}", 
        "[FieldProp] public static readonly System.Windows.DependencyProperty PropProperty")]
    [InlineData("[GenerateDP][field:FieldProp][property:PropProp] private void OnPropChanged(int new){}", 
        "[PropProp] public int Prop")]
    [InlineData("[GenerateDP(Attached=true)][field:FieldProp][method:MethProp] private void OnPropChanged(int new){}", 
        "[MethProp] public static int GetProp(System.Windows.DependencyObject")]
    [InlineData("[GenerateDP(Attached=true)][field:FieldProp][method:MethProp] private void OnPropChanged(int new){}", 
        "[MethProp] public static void SetProp(System.Windows.DependencyObject")]
    public void CopyAppropriateAttributes(string prompt, string defaultText)
    {
        var res = RunTest(prompt);
        res.LastFile().AssertContainsIgnoreWhiteSpace(defaultText);
            
    }
    [Fact]
    public void GenerateAttachedFromDeclaration()
    {
        var res = RunTest(@"
              [GenerateDP] public static readonly System.Windows.DependencyProperty NovelProperty =
                System.Windows.DependencyProperty.RegisterAttached(""Novel"", typeof(int),
                typeof(c), new System.Windows.FrameworkPropertyMetadata(0));");
            
        res.LastFile().AssertDoesNotContain(".Register");
        res.LastFile().AssertContains("public static int GetNovel(System.Windows.DependencyObject obj) =>");
        res.LastFile().AssertContains("(int)obj.GetValue(Outer.C.NovelProperty);");
        res.LastFile().AssertContains("public static void SetNovel(System.Windows.DependencyObject obj, int value) =>");
        res.LastFile().AssertContains("obj.SetValue(Outer.C.NovelProperty, value);");
    }

    private void MultiContentTest(string source, params string[] contents)
    {
        var res = RunTest(source);
        foreach (var content in contents)
        {
            res.LastFile().AssertContains(content);
        }
            
    }

}