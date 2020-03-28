using System;
using Accord.Statistics.Distributions;
using Accord.Statistics.Distributions.Univariate;

namespace Melville.Linq.Statistics.HypothesisTesting
{
  public class Statistic
  {
    protected readonly IUnivariateDistribution Distribution;
    protected readonly double innerValue;
    public Statistic(IUnivariateDistribution distribution, double innerValue)
    {
      Distribution = distribution;
      this.innerValue = innerValue;
    }
    public double OneSidedLessP => Distribution.DistributionFunction(innerValue);
    public double OneSidedGreaterP => 1.0 - OneSidedLessP;

    public double TwoTailedP
    {
      get
      {
        var absT = Math.Abs(innerValue);
        return Distribution.DistributionFunction(-absT) + (1 - Distribution.DistributionFunction(absT));
      }
    }
  }
  public class TStatistic:Statistic
  {
    public double T => innerValue;
    public double DegreesOfFreedom { get; }

    public TStatistic(double t, double degreesOfFreedom):base(new TDistribution(degreesOfFreedom), t)
    {
      DegreesOfFreedom = degreesOfFreedom;
    }
  }

  public class ChiSquaredStatisic 
  {
    public double ChiSquared { get; }
    public int DegreesOfFreedom { get; }
    public double P => 1.0 - 
      new ChiSquareDistribution(DegreesOfFreedom)
      .DistributionFunction(ChiSquared);

    public ChiSquaredStatisic(double innerValue, int freedom)
    {
      ChiSquared = innerValue;
      DegreesOfFreedom = freedom;
    }
  }

  public class NormalStatistic : Statistic
  {
    public double Mean { get; }
    public double StdDeviation { get; }
    public double ZScore => innerValue;

    public NormalStatistic(double mean, double stdDeviation, double value) : base(new NormalDistribution(mean, stdDeviation), value)
    {
      Mean = mean;
      StdDeviation = stdDeviation;
    }

    public override string ToString() => $"T: {ZScore:0.0###} p = {TwoTailedP:0.0##}";
  }

  public class UStatistic : Statistic
  {
    public double UMax { get; }
    public double UMin { get; }
    public double ZScore => innerValue;
    public UStatistic(double u1, double u2, double zScore) : 
      base(new NormalDistribution(0, 1), zScore)
    {
      UMax = Math.Max(u1, u2);
      UMin = Math.Min(u1, u2);
    }
  }
}