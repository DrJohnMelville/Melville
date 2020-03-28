using System;
using Accord.Statistics.Testing;

namespace Melville.Linq.Statistics.HypothesisTesting
{
  /// <summary>
  /// Very thin wrapper arround MannWhitneyWIlcoxonTest so it shows up well in the dump.
  /// </summary>
  public class RankSumResult
  {
    private readonly MannWhitneyWilcoxonTest result;
    public RankSumResult(MannWhitneyWilcoxonTest result, int n1, int n2)
    {
      this.result = result;
      N1 = n1;
      N2 = n2;
    }

    public int N1 { get; }
    public int N2 { get; }
    public double RankSum1 => result.RankSum1;
    public double RankSum2 => result.RankSum2;
    public double UMin => Math.Min(result.Statistic1, result.Statistic2);
    public double UMax => Math.Max(result.Statistic1, result.Statistic2);
    public double CriticalValue => result.CriticalValue;
    public double PValue => result.PValue;
  }
}