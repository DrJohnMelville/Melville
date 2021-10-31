using System;
using System.Collections.Generic;
using Melville.Generators.INPC.AstUtilities;
using Xunit;

namespace Melville.Generators.INPC.Test.AstUtilities;

public class TypeOperationTest
{
    [Theory]
    [InlineData("byte", typeof(byte))]
    [InlineData("sbyte", typeof(sbyte))]
    [InlineData("char", typeof(char))]
    [InlineData("float", typeof(float))]
    [InlineData("double", typeof(double))]
    [InlineData("int", typeof(int))]
    [InlineData("uint", typeof(uint))]
    [InlineData("long", typeof(long))]
    [InlineData("ulong", typeof(ulong))]
    [InlineData("short", typeof(short))]
    [InlineData("ushort", typeof(ushort))]
    [InlineData("object", typeof(object))]
    [InlineData("string", typeof(string))]
    [InlineData("System.Collections.Generic.IList<int>", typeof(IList<int>))]
    [InlineData("Melville.Generators.INPC.Test.AstUtilities.TypeOperationTest", typeof(TypeOperationTest))]
    public void CheckTypeName(string name, Type type)
    {
        Assert.Equal(name, type.FullyQualifiedName());
    }
}