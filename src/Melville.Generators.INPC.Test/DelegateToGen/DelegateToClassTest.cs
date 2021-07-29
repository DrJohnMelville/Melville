using Melville.Generators.INPC.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen
{
    public class DelegateToClassTest
    {
        private GeneratorTestBed RunTest(string classDecl, string classMembers, string s) => 
            new(new DelegateToGenerator(), $@"
using Melville.INPC;
namespace Outer
{{
    public {classDecl} Delegated
    {{"+classMembers+@"}
    public partial class C: Delegated {" + s + @"
}
}");

        [Fact]
        public void DelegateToClass()
        {
            var res = RunTest("class", "public virtual int Foo()=>1;", "[DelegateTo] Delegated bar;");
            res.FileContains("C.DelegateToGeneration.cs", "public override int Foo() => this.bar.Foo();");            

        }

        [Fact] public void ForwardOverload()
        {
            var res = RunTest("class", "public virtual int Foo()=>1; public virtual int Foo(int i)=>i;", 
                "[DelegateTo] Delegated bar; public override int Foo(int i)=>1;");
            res.FileContains("C.DelegateToGeneration.cs", "public override int Foo() => this.bar.Foo();");            

        }
        [Fact] public void ForwardOverloadNumber()
        {
            var res = RunTest("class", "public virtual int Foo(int i, int j)=>1; public virtual int Foo(int i)=>i;", 
                "[DelegateTo] Delegated bar; public override int Foo(int i)=>1;");
            res.FileContains("C.DelegateToGeneration.cs", "public override int Foo(int i, int j) => this.bar.Foo(i, j);");            

        }
        [Fact] public void ForwardOverloadType()
        {
            var res = RunTest("class", "public virtual int Foo(double d)=>1; public virtual int Foo(int i)=>i;", 
                "[DelegateTo] Delegated bar; public override int Foo(int i)=>1;");
            res.FileContains("C.DelegateToGeneration.cs", "public override int Foo(double d) => this.bar.Foo(d);");            

        }
        [Fact]
        public void DoNotForwardExistingMethod()
        {
            var res = RunTest("class", "public virtual int Foo()=>1;", "[DelegateTo] Delegated bar; public override int Foo()=>1;");
            res.FileDoesNotContain("C.DelegateToGeneration.cs", "Foo");
        }
        [Fact]
        public void DelegateAbstractMethod()
        {
            var res = RunTest("abstract class", "public abstract int Foo();", "[DelegateTo] Delegated bar;");
            res.FileContains("C.DelegateToGeneration.cs", "public override int Foo() => this.bar.Foo();");            

        }
        [Fact]
        public void DelegateProtectedMethod()
        {
            var res = RunTest("class", "protected virtual int Foo()=>1;", "[DelegateTo] Delegated bar;");
            res.FileDoesNotContain("C.DelegateToGeneration.cs", "Foo");            

        }
        [Fact]
        public void DoNotDelegateNonVirtualMethods()
        {
            var res = RunTest("class", "public int Foo()=>1;", "[DelegateTo] Delegated bar;");
            res.FileDoesNotContain("C.DelegateToGeneration.cs", "Foo");            

        }
    }
}