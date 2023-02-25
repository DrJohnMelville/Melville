using System.Text.Json.Serialization;
using Melville.Generators.INPC.ProductionGenerators.INPC;
using Melville.INPC;
using Moq.Protected;
using Xunit;

namespace Melville.Generators.INPC.Test.UnitTests;

public class ImplementNotifyTest
{
    private const string AttrDecl = """
    namespace Melville.INPC
    {
        public sealed class AutoNotifyAttribute : System.Attribute
        {
            public string PropertyModifier { get; set;} = "";
        }
    }
    """;
    [Fact]
    public void MultipleNameSpaces()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            """
            using Melville.INPC;
            namespace Outer{
            namespace NM
            {
              public partial class C
              {  
                [AutoNotify] private int intProp,ip2;
              }
            }
            }
            """ + AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("namespace Outer");
        tb.LastFile().AssertContains("namespace NM");
    }

    [Fact]
    public void MultipleWrappingClasses()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  public partial class C
  {
    public partial class D{ 
    [AutoNotify] private int intProp,ip2;
  }}"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("public partial class C");
        tb.LastFile().AssertContains("public partial class D: Melville.INPC.IExternalNotifyPropertyChanged");
    }

    [Fact]
    public void MultipleVarsPerDecl()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
using Melville.INPC;
  public partial class C
  { 
    [AutoNotify] private int intProp,ip2;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("public int IntProp");
        tb.LastFile().AssertContains("public int Ip2");

    }

    [Fact]
    public void DoNotTouchUnmarkedClass()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
using Melville.INPC;
  public partial class C
  { 
     private int intProp,ip2;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.NoSuchFile("INPC.C.cs");
    }
    [Fact]
    public void ElaborateMarkedClass()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
using Melville.INPC;
  [AutoNotify]
  public partial class C
  { 
     private int intProp,ip2;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertDoesNotContain("AutoNotify");
        tb.LastFile().AssertContains("void Melville.INPC.IExternalNotifyPropertyChanged.OnPropertyChanged(string propertyName)");
    }

    [Fact]
    public void DoNotTouchClassWithWrongAttribute()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
  namespace A2 {
   public class AutoNotifyAttribute : Attribute{}
}
  public partial class C
  { 
     [A2.AutoNotify] private int intProp,ip2;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.NoSuchFile("INPC.C.cs");
    }

    [Fact]
    void DoNotTouchUnmarkedField()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
using Melville.INPC;
  public partial class C
  { 
     private int intProp;
     [AutoNotify] private int ip2;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("public int Ip2");
        tb.LastFile().AssertDoesNotContain("public int IntProp");
    }

    [Fact]
    public void UseFullyQualifiedAttribute()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
using Melville.INPC;
  public partial class C
  { 
     private int intProp;
     [Melville.INPC.AutoNotify] private int ip2;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("public int Ip2");
    }

    [Fact]
    public void AllowQualifiedAttribute()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
  public partial class C
  { 
     private int intProp;
     [Melville.INPC.AutoNotify] private int ip2;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("public int Ip2");
        tb.LastFile().AssertDoesNotContain("public int IntProp");
    }

    [Fact]
    public void AllowExplicitAttribute()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
  public partial class C
  { 
     private int intProp;
     [Melville.INPC.AutoNotifyAttribute] private int ip2;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("public int Ip2");
        tb.LastFile().AssertDoesNotContain("public int IntProp");
    }

    [Fact]
    public void UseExistingNotifyMethod()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
  public partial class C
  { 
     public void OnPropertyChanged(string name){}
     private int intProp;
     [Melville.INPC.AutoNotifyAttribute] private int ip2;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertDoesNotContain("IExternalNotifyPropertyChange");
    }

    [Fact]
    public void ImplementCustomInterface()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            $@"
  public interface INot:INotifyPropertyChanged{{
     public void OnPropertyChanged(string name);  
  }}
  public partial class C:INot
  {{ 
     private int intProp;
     [Melville.INPC.AutoNotifyAttribute] private int ip2;
  }}"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("void INot.OnPropertyChanged(string");
        tb.LastFile().AssertDoesNotContain("IExternalNotifyPropertyChange");
    }

    [Fact]
    public void RespectExistingCustomInterfaceIImplementation()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            $@"namespace NS {{
  public interface INot:INotifyPropertyChanged{{
     void OnPropertyChanged(string name);  
  }}
  public partial class C:INot
  {{ 
     void INot.OnPropertyChanged(string name){{}}
     private int intProp;
     [Melville.INPC.AutoNotifyAttribute] private int ip2;
  }}}}"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("((NS.INot)this).OnPropertyChanged(\"Ip2\")");
        tb.LastFile().AssertDoesNotContain("void NS.INot.OnPropertyChanged(string");
        tb.LastFile().AssertDoesNotContain("IExternalNotifyPropertyChange");
    }
    [Fact]
    public void UseInheritedInterfaceMethod()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            $@" namespace NS {{
    public interface I {{ void OnPropertyChanged(string name);}}
  public class A: I {{
             void I.OnPropertyChanged(string name){{}}
            }}
  public partial class C: A
  {{ 
     private int intProp;
     [Melville.INPC.AutoNotifyAttribute] private int ip2;
  }}}}" + AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertDoesNotContain("IExternalNotifyPropertyChange");
        tb.LastFile().AssertContains("((NS.I)this).OnPropertyChanged");
    }
    [Fact]
    public void Generate()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            """
            using Melville.INPC;
              public partial class C
              { 
                [AutoNotify] private int ip2;
              }
            " + AttrDecl);
                    tb.AssertNoDiagnostics();
                    tb.FileEqual("INPC.C.cs",
                        @"using Melville.INPC;
            public partial class C: Melville.INPC.IExternalNotifyPropertyChanged 
            {
                public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
                void Melville.INPC.IExternalNotifyPropertyChanged.OnPropertyChanged(string propertyName)
                {
                    this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
                public int Ip2
                {
                    get => this.ip2;
                    set
                    {
                        this.ip2 = value;
                        ((Melville.INPC.IExternalNotifyPropertyChanged)this).OnPropertyChanged(""Ip2"");
                    }
                }
            }
            """, typeof(AutoNotifyAttribute));
    }

    [Theory]
    [InlineData("private void OnIpChanged(int old, int newVal){} private int IpSetFilter(int a)=>a;",
        "this.OnIpChanged(this.ip, this.ip = this.IpSetFilter(value));", "this.ip = value;")]
    [InlineData("private void OnIpChanged(int old, int newVal){}",
        "this.OnIpChanged(this.ip, this.ip = value);", "this.ip = value;")]
    [InlineData("private void OnIpChanged(int newVal){}", 
        "this.OnIpChanged(this.ip);", "xxxx;")]
    [InlineData("private void OnIpChanged(){}", "this.ip = value;", "gbjojh")]
    [InlineData("private void OnIpChanged(){}", "this.OnIpChanged();", "gbjojh")]
    [InlineData(" ", "this.ip = value;", "this.OnIpChanged();")]
    public void OnPropertyChanged(string methodDecl, string included, string excluded)
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            $@"
using Melville.INPC;
namespace NM
{{
  public class C
  {{ 
    {methodDecl}
    [AutoNotify] private int ip;
  }}
}}" + AttrDecl, typeof(AutoNotifyAttribute));
        tb.LastFile().AssertContains(included);
        tb.LastFile().AssertDoesNotContain(excluded);
    }

    [Fact]
    public void NameSpaceBlock()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
using Melville.INPC;
namespace NM
{
  public class C
  { 
    [AutoNotify] private int ip;
  }
}"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.LastFile().AssertContains("namespace NM\r\n{");
        tb.LastFile().AssertDoesNotContain("namespace NM;");
            
    }
    [Fact]
    public void NameSpaceDecl()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
using Melville.INPC;
namespace NM.A.B
{
public class C
{ 
  [AutoNotify] private int ip;
}
}"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.LastFile().AssertContains("namespace NM.A.B\r\n{");
        tb.LastFile().AssertDoesNotContain("namespace NM.A.B;");
            
    }

    [Fact]
    public void ClassMustBePartial()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"
using Melville.INPC;
namespace NM
{
  public class C
  { 
    [AutoNotify] private int intProp;
  }
}"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertDiagnosticCount(1);
    }

    [Fact]
    public void InheritsFromAnotherGeneratedType()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
    public partial class ParentINPC
    {
        [AutoNotify] private int x;
    }
    public partial class ImplementInheritedINPC: ParentINPC
    {
        [AutoNotify] private int y;
    }"+AttrDecl, typeof(AutoNotifyAttribute));
          tb.LastFile().AssertContains("public partial class ImplementInheritedINPC \r\n{");
    }

    [Fact]
    public void AncestorImplementsOnPropertyChanged()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @" using Melville.INPC;  
  public partial class ParentINPC
    {
        protected void OnPropertyChanged(string property) {}
    }
    public partial class ImplementInheritedINPC: ParentINPC
    {
        [AutoNotify] private int y;
    }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.LastFile().AssertContains("public partial class ImplementInheritedINPC \r\n{");
    }

    [Fact]
    public void ClassImplementsOnPropertyChanged()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  public partial class ImplementInheritedINPC
    {
        protected void OnPropertyChanged(string property) {}
        [AutoNotify] private int y;
    }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.LastFile().AssertContains("public partial class ImplementInheritedINPC \r\n{");
    }

    [Theory]
    [InlineData("[AutoNotify] int FindY {get{return Y;}}")]
    [InlineData("[AutoNotify] int FindY => Y")]
    [InlineData("[AutoNotify] string FindY => Y.GetType().ToString")]
    [InlineData("[AutoNotify] string FindY => this.Y.GetType().ToString")]
    public void PropegateToReadOnlyPropertyImpl(string propDecl)
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  public partial class ImplementInheritedINPC
    {
        protected void OnPropertyChanged(string property) {}
        [AutoNotify] private int y;
        "+propDecl+@"
    }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.LastFile().AssertContains(@"OnPropertyChanged(""FindY"");");
    }

    [Fact]
    public void DoNotPropegateOtherCalls()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  public partial class ImplementInheritedINPC
    {
        protected void OnPropertyChanged(string property) {}
        [AutoNotify] private int y;
        private ImplementInheritedINPC other;
        [AutoNotify] public int FindY {get {return other?.Y;}}
    }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.LastFile().AssertDoesNotContain(@"OnPropertyChanged(""FindY"");");
    }
        
    [Fact]
    public void GenericVariable()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  using System.Collections.Generic;
  public partial class C
  {
    [AutoNotify] private List<int> ints;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("System.Collections.Generic.List<int>");
    }
    [Fact]
    public void Nullable()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  using System.Collections.Generic;
  public partial class C
  {
    [AutoNotify] private int? intProp;
    [AutoNotify] private string? stringProp;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("int? IntProp");
        tb.LastFile().AssertContains("string? StringProp");
    }
    [Fact]
    public void AmbigiousFileNames()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  public partial class C
  {
    [AutoNotify] private int? intProp;
  }
  public partial class C<T>: C {
     [AutoNotify] private T tProp;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.FromName("C.50C5D5C.g.cs").AssertContains("int? IntProp");
        tb.FromName("C.74C0903A.g.cs").AssertContains("T TProp");
    }
    [Fact]
    public void IncludeAttribute()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
          """
          using Melville.INPC;
          using System.Collections.Generic;
          public class NewPropAttribute:System.Attribute {public NewPropAttribute(int i){}}
          public partial class C
          {
            [AutoNotify]
            [property:NewProp(1)]
            private int integer;
          }
          """+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("[NewProp(1)]\r\n    public int Integer");
        tb.LastFile().AssertDoesNotContain("[AutoNotify]");
    }
    [Fact]
    public void SetFilter()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  using System.Collections.Generic;
  public partial class C
  {
    [AutoNotify] private int integer;
    private int IntegerSetFilter(int i) => i;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("this.integer = this.IntegerSetFilter(value);");
    }
    [Fact]
    public void GetFilter()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  using System.Collections.Generic;
  public partial class C
  {
    [AutoNotify] private int integer;
    private int IntegerGetFilter(int i) => i;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("=> this.IntegerGetFilter(this.integer);");
    }
    [Fact]
    public void CustomPropertyModifiers()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  using System.Collections.Generic;
  public partial class C
  {
    [AutoNotify (PropertyModifier = ""protected virtual"")] private int integer;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("protected virtual int Integer");
    }
    [Fact]
    public void ClassWithGenericConstraint()
    {
        var tb = new GeneratorTestBed(new INPCGenerator(),
            @"using Melville.INPC;
  public interface IInt{}
  public partial class C<T> where T: IInt
  {
    [AutoNotify] private int integer;
  }"+AttrDecl, typeof(AutoNotifyAttribute));
        tb.AssertNoDiagnostics();
        tb.LastFile().AssertContains("public partial class C<T>: Melville.INPC.IExternalNotifyPropertyChanged where T: IInt");
    }

}