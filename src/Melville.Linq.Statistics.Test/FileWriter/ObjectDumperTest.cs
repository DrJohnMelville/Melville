using System;
using System.Linq;
using Melville.Linq.Statistics.FileWriter;
using Xunit;

namespace Test.FileWriter
{
  public class ObjectDumperTest
  {
    [Fact]
    public void DumpSimpleObject()
    {
      var ret = ObjectTableFormatter.Dump(new[]
      {
        new {A = "A1", B = "B1"},
        new {A = "A2", B = "B2"},
      }).ToList();

      Assert.Equal("A", ret[0][0]);
      Assert.Equal("B", ret[0][1]);
      Assert.Equal("A1", ret[1][0]);
      Assert.Equal("B1", ret[1][1]);
      Assert.Equal("A2", ret[2][0]);
      Assert.Equal("B2", ret[2][1]);    
    }

    [Fact]
    public void UnPascalCase()
    {
      var ret = ObjectTableFormatter.Dump(new[]
      {
        new {ThisIsPascalCase = "A1", B = "B1"},
      }).ToList();

      Assert.Equal("This Is Pascal Case", ret[0][0]);
    }

    [Fact]
    public void Types()
    {
      var ret = ObjectTableFormatter.Dump(new[]
      {
        new
        {
          Int = 1, Double = 1.0, Date = DateTime.Now, Null = (string?)null,
          IntString = "10", DoubleString="10.2", String="xxYY"
        },
      }).ToList();

      Assert.True(ret[1][0] is int);
      Assert.True(ret[1][1] is Double);
      Assert.True(ret[1][2] is string);
      Assert.Null(ret[1][3]);
      Assert.True(ret[1][4] is int);
      Assert.True(ret[1][5] is double);
      Assert.True(ret[1][6] is string);
    }
  }
}