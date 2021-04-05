using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Linq.Statistics.DescriptiveStats;

namespace Melville.Linq.Statistics.HypothesisTesting
{
  public static class TTest
  {
    public static TStatistic OneSample(double hypothesizedMean, double sampleMean, double sampleVariance,
      int sampleSize)
    {
      return new TStatistic((sampleMean - hypothesizedMean) / (sampleVariance / Math.Sqrt(sampleSize)),
        sampleSize - 1);
    }

    public static TStatistic OneSample(double hypothesizedMean, IEnumerable<double> values)
    {
      var localSamples = values.ToList();
      var stats = localSamples.MeanAndStandardDeviation(false);
      return OneSample(hypothesizedMean, stats.Mean, stats.StdDev, localSamples.Count);
    }

    public static TStatistic TwoSample(double mean1, double stdDev1, int n1,
      double mean2, double stdDev2, int n2)
    {
      var pooledVariance = (stdDev1 * stdDev1 / n1) + (stdDev2 * stdDev2 / n2);
      var stvDevDelta = Math.Sqrt(pooledVariance);
      double denomHalf(double s, int n) => Math.Pow(s * s / n, 2) / (n - 1);
      var df = (pooledVariance * pooledVariance) /
               ((denomHalf(stdDev1, n1)) + (denomHalf(stdDev2, n2)));
      return new TStatistic((mean1-mean2)/stvDevDelta, df);
    }

    public static TStatistic TwoSample(IEnumerable<double> sample1, IEnumerable<double> sample2)
    {
      var list1 = sample1.ToList();
      var list2 = sample2.ToList();
      var stat1 = list1.MeanAndStandardDeviation();
      var stat2 = list2.MeanAndStandardDeviation();
      return TwoSample(stat1.Mean, stat1.StdDev, list1.Count, stat2.Mean, stat2.StdDev, list2.Count);
    }

    public static TStatistic PairedTTest<T>(IEnumerable<T> data, Func<T, double> a, Func<T, double> b)
    {
      return OneSample(0, data.Select(i=>b(i)-a(i)));
    }
  }
}