#nullable disable warnings
using  System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Melville.Hacks;
using Xunit;

namespace Melville.Mvvm.Test.Functional;

public sealed class GetPropertyNamesTest
{
  public IEnumerable<string> GetNames<T1,T2>(Expression<Func<T1, T2>> expr) => GetPropertyNames.FromExpression(expr);
  [Fact]
  public void ExtractSingleName()
  {
    Assert.Equal(new[] {"Length"}, GetNames((string s)=>s.Length));
  }
  [Fact]
  public void ExtractTwoNames()
  {
    Assert.Equal(new[] {"Data", "Message"}, GetNames((Exception e)=>e.Data + e.Message));
  }
  [Fact]
  public void ExtractChain()
  {
    Assert.Equal(new[] {"Count", "Keys", "Data"}, GetNames((Exception e)=>e.Data.Keys.Count));
  }
  [Fact]
  public void EliminateDuplicates()
  {
    Assert.Equal(new[] {"Message"}, GetNames((Exception e)=>e.Message + e.Message));
  }
  [Fact]
  public void ExtractSingleNameAndIgnoreMethods()
  {
    Assert.Equal(new[] {"Length"}, GetNames((string s)=>s.Equals(s.Length.ToString())));
  }

}