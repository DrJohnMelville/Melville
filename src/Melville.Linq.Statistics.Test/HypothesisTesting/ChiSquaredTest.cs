using Xunit;
using Melville.Linq.Statistics.HypothesisTesting;

namespace Test.HypothesisTesting
{
  public sealed class ChiSquaredTest
  {
    [Fact]
    public void table2x2()
    {
      #region data
      var data = new[]
      {
        new { Aids = "N", Pref = "M"},
        new { Aids = "Y", Pref = "B"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "B"},
        new { Aids = "Y", Pref = "F"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "M"},
        new { Aids = "Y", Pref = "F"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "Y", Pref = "B"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "B"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "M"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "Y", Pref = "M"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "Y", Pref = "B"},
        new { Aids = "Y", Pref = "M"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "Y", Pref = "M"},
        new { Aids = "N", Pref = "F"},
        new { Aids = "Y", Pref = "M"},
        new { Aids = "N", Pref = "F"}
      };
      #endregion

      var stat = data.ChiSquared(i=>i.Aids, i=>i.Pref);
      Assert.Equal(7.66, stat.ChiSquared, 2);
      Assert.Equal(0.022, stat.P, 3);
    }

    /// <summary>
    /// This turns a potentially interned string into a unique string by adding the empty string
    /// </summary>
    /// <param name="str">input</param>
    /// <returns>A unique string object</returns>
    private static string PreventInterning(string str)
    {
      return str+"";
    }
  }
}