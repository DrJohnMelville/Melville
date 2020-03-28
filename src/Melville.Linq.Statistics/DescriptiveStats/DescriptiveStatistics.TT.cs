
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Melville.Linq.Statistics.DescriptiveStats
{
  public static partial class DescriptiveStatsImplementation
  {
        public static (double Mean, double StdDev) MeanAndStandardDeviation(this IEnumerable<double> items,
      bool population = false)
    {
      {
        // ref: http://warrenseen.com/blog/2006/03/13/how-to-calculate-standard-deviation/
        double mean = 0.0;
        double sum = 0.0;
        double stdDev = 0.0;
        int n = 0;
        foreach (double val in items)
        {
          n++;
          double delta = val - mean;
          mean += delta / n;
          sum += delta * (val - mean);
        }
        if (1 < n)
          stdDev = Math.Sqrt(sum / (n - (population?0:1)));

        return (mean,stdDev);
      }
    }

      public static double StandardDeviation(this IEnumerable<double> items,
      bool population = false) => MeanAndStandardDeviation(items, population).StdDev;
        public static (double Mean, double StdDev) MeanAndStandardDeviation(this IEnumerable<int> items,
      bool population = false)
    {
      {
        // ref: http://warrenseen.com/blog/2006/03/13/how-to-calculate-standard-deviation/
        double mean = 0.0;
        double sum = 0.0;
        double stdDev = 0.0;
        int n = 0;
        foreach (double val in items)
        {
          n++;
          double delta = val - mean;
          mean += delta / n;
          sum += delta * (val - mean);
        }
        if (1 < n)
          stdDev = Math.Sqrt(sum / (n - (population?0:1)));

        return (mean,stdDev);
      }
    }

      public static double StandardDeviation(this IEnumerable<int> items,
      bool population = false) => MeanAndStandardDeviation(items, population).StdDev;
  
  }
}