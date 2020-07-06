using System;

namespace Melville.Linq.Statistics.HypothesisTesting
{
    public static class ProportionStatistics
    {
        public static NormalStatistic DifferenceOfProportions(int count1, int total1, int count2, int total2)
        {
            var p1 = count1 * 1.0 / total1;
            var p2 = count2 * 1.0 / total2;

            var pooled = (count1 + count2) * 1.0 / (total1 + total2);
            var se = Math.Sqrt(pooled * (1.0 - pooled) * ((1.0 / total1) + (1.0 / total2)));
            var z = (p1 - p2) / se;
            return new NormalStatistic(0.0, 1.0, z);
        }
    }
}