#nullable disable warnings
using  System.Collections.Generic;
using Melville.Hacks;
using Melville.Linq;
using Xunit;

namespace Melville.Mvvm.Test.Functional
{
  public sealed class ListExtensionsTest
  {
    [Fact]
    public void AddRangeTest()
    {
      IList<int> l = new List<int>();
      l.AddRange(new []{1,2,3,4});
      Assert.Equal(4, l.Count);
      Assert.Equal(1, l[0]);
      Assert.Equal(2, l[1]);
      Assert.Equal(3, l[2]);
      Assert.Equal(4, l[3]);
            
    }
 
  }
}