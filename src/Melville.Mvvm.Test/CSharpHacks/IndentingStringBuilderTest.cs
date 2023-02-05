using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Melville.Hacks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Xunit;

namespace Melville.Mvvm.Test.CSharpHacks;

public class IndentingStringBuilderTest
{
    private void AssertResult(string expected, Action<IndentingStringBuilder> action)
    {
        var sut = new IndentingStringBuilder();
        action(sut);
        Assert.Equal(expected, sut.ToString());
    }

    [Fact] public void Empty() => AssertResult("", _ => { });
    [Fact] public void Literal() => AssertResult("ABC", i => i.Append("ABC"));
    [Fact] public void Formatted() => AssertResult("YYY + 11\r\n", i => i.AppendLine($"YYY + {11}"));
    [Fact] public void Number() => AssertResult("4", i => i.Append(4));
    [Fact] public void AppendLine() => AssertResult("4\r\n", i => i.AppendLine("4"));

    [Fact]
    public void SingleIndent() => AssertResult("""
        l1
            l2
        l1

        """, i =>
        i.AppendLine("l1")
            .BeginIndent()
            .AppendLine("l2")
            .EndIndent()
            .AppendLine("l1")
    );

    [Fact] public void Double() => AssertResult("""
        l1
            l2
                l3
                l3
        l1

        """, i =>
    {
        i.AppendLine("l1")
        .BeginIndent()
        .AppendLine("l2")
        .BeginIndent()
        .AppendLine("l3")
        .AppendLine("l3")
        .EndIndent()
        .EndIndent()
        .AppendLine("l1");
    });    
    
    [Fact] public void DoNotWritePrefixUntilNeeded() => AssertResult("""
        l1
            l2
        
        """, i =>
    {
        i.AppendLine("l1");
        i.BeginIndent();
        i.AppendLine("l2");
    });

    [Fact] public void OpenBlock() => AssertResult("""
        l1
            l2
        l3

        """, i =>
    {
        i.AppendLine("l1");
        using (i.OpenBlock())
        {
            i.AppendLine("l2");
        }
        i.AppendLine("l3");
    });
}