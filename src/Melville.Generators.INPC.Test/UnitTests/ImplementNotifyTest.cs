using Melville.Generators.INPC.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.UnitTests
{
    public class ImplementNotifyTest
    {
        [Fact]
        public void MultipleNameSpaces()
        {
            var tb = new GeneratorTestBed(new INPCGenerator(),
                @"
using Melville.INPC;
namespace Outer{
namespace NM
{
  public partial class C
  { 
    [AutoNotify] private int intProp,ip2;
  }
}
}");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "namespace Outer");
            tb.FileContains("C.INPC.cs", "namespace NM");
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
  }}");
            tb.AssertNoDiagnostics();
            tb.FileContains("D.INPC.cs", "public partial class C");
            tb.FileContains("D.INPC.cs",
                "public partial class D : Melville.INPC.IExternalNotifyPropertyChanged");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "public int IntProp");
            tb.FileContains("C.INPC.cs", "public int Ip2");

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
  }");
            tb.AssertNoDiagnostics();
            tb.NoSuchFile("C.INPC.cs");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileDoesNotContain("C.INPC.cs", "AutoNotify");
            tb.FileContains("C.INPC.cs", 
                "void Melville.INPC.IExternalNotifyPropertyChanged.OnPropertyChanged(string propertyName)");
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
  }");
            tb.AssertNoDiagnostics();
            tb.NoSuchFile("C.INPC.cs");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "public int Ip2");
            tb.FileDoesNotContain("C.INPC.cs", "public int IntProp");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "public int Ip2");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "public int Ip2");
            tb.FileDoesNotContain("C.INPC.cs", "public int IntProp");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "public int Ip2");
            tb.FileDoesNotContain("C.INPC.cs", "public int IntProp");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileDoesNotContain("C.INPC.cs", "IExternalNotifyPropertyChange");
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
  }}");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "void INot.OnPropertyChanged(string");
            tb.FileDoesNotContain("C.INPC.cs", "IExternalNotifyPropertyChange");
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
  }}}}");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "((NS.INot)this).OnPropertyChanged(\"Ip2\")");
            tb.FileDoesNotContain("C.INPC.cs", "void NS.INot.OnPropertyChanged(string");
            tb.FileDoesNotContain("C.INPC.cs", "IExternalNotifyPropertyChange");
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
  }}}}");
            tb.AssertNoDiagnostics();
            tb.FileDoesNotContain("C.INPC.cs", "IExternalNotifyPropertyChange");
            tb.FileContains("C.INPC.cs", "((NS.I)this).OnPropertyChanged");
        }
        [Fact]
        public void Generate()
        {
            var tb = new GeneratorTestBed(new INPCGenerator(),
                @"
using Melville.INPC;
  public partial class C
  { 
    [AutoNotify] private int ip2;
  }
");
            tb.AssertNoDiagnostics();
            tb.FileEqual("C.INPC.cs",
                @"#nullable enable
using Melville.INPC;
public partial class C : Melville.INPC.IExternalNotifyPropertyChanged
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
            var ___LocalOld = this.ip2;
            this.ip2 = value;
            WhenIp2Changes(___LocalOld, value);
            ((Melville.INPC.IExternalNotifyPropertyChanged)this).OnPropertyChanged(""Ip2"");
        }
    }
    partial void WhenIp2Changes(int oldValue, int newValue);
}
");
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
}");
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
    }");
            tb.FileContains("ImplementInheritedINPC.INPC.cs",
                "public partial class ImplementInheritedINPC \r\n{");
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
    }");
            tb.FileContains("ImplementInheritedINPC.INPC.cs",
                "public partial class ImplementInheritedINPC \r\n{");
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
    }");
            tb.FileContains("ImplementInheritedINPC.INPC.cs",
                "public partial class ImplementInheritedINPC \r\n{");
        }

        [Fact]
        public void PropegateToReadOnlyPropertyImpl()
        {
            var tb = new GeneratorTestBed(new INPCGenerator(),
                @"using Melville.INPC;
  public partial class ImplementInheritedINPC
    {
        protected void OnPropertyChanged(string property) {}
        [AutoNotify] private int y;
        [AutoNotify] public int FindY {get {return Y;}}
    }");
            tb.FileContains("ImplementInheritedINPC.INPC.cs",
                @"OnPropertyChanged(""FindY"");");
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
    }");
            tb.FileDoesNotContain("ImplementInheritedINPC.INPC.cs",
                @"OnPropertyChanged(""FindY"");");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "System.Collections.Generic.List<int>");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "int? IntProp");
            tb.FileContains("C.INPC.cs", "string? StringProp");
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
  }");
            tb.AssertNoDiagnostics();
            tb.FileContains("C.INPC.cs", "int? IntProp");
            tb.FileContains("C.INPC.1.cs", "T TProp");
        }

    }
}