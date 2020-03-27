namespace Melville.Linq.Statistics.DescriptiveStats
{
  public static class DoubleExtensions
  {
    public static bool IsValidAndFinite(this double d) => !(double.IsInfinity(d) || double.IsNaN(d));
  }
}