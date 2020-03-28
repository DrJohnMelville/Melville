using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Accord.Statistics.Testing;
using Melville.CurveFit;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.Graphics;

namespace Melville.Linq.Statistics.HypothesisTesting
{
  public static class FunctionalStatistics
  {
    #region Data Mainpulators

    private static (List<double> FirstSample, List<double> SecondSample) SegmentData<T>(
      IEnumerable<T> items, Func<T, double> value, Func<T, bool> predicate)
    {
      var s1 = new List<double>();
      var s2 = new List<double>();
      foreach (var item in items)
      {
        (predicate(item) ? s1 : s2).Add(value(item));
      }
      var ret = (FirstSample: s1, SecondSample: s2);
      return ret;
    }


    #endregion
    #region T Test

    public static TStatistic TTest<T>(this IEnumerable<T> data, Func<T, double> a, Func<T, double> b)
    {
      return HypothesisTesting.TTest.PairedTTest(data, a, b);
    }

    public static TStatistic TTest<T>(this IEnumerable<T> data,
      Func<T, bool> discriminator, Func<T, double> datum)
    {
      var partition = data.GroupBy(discriminator).AsList();
      if (partition.Count < 2) throw new InvalidDataException("One or more group is empty for the T Test");
      return HypothesisTesting.TTest.TwoSample
        (partition[0].Select(datum), partition[1].Select(datum));
    }
    public static TStatistic TTest(this IEnumerable<Double> data, double hypotensizedMean)
    {
      return HypothesisTesting.TTest.OneSample(hypotensizedMean, data);
    }


    #endregion

    #region RankSum

    public static RankSumResult RankSum<S>(this IEnumerable<S> items, Func<S, double?> keyFunc,
      Func<S, bool?> testFunction, TwoSampleHypothesis hypothesis = TwoSampleHypothesis.ValuesAreDifferent)
    {
      var filter = UnknownFilter.Create(items);
      filter.AddFilter(keyFunc);
      filter.AddFilter(testFunction);
      return RankSum(filter.FilteredResult(), i => keyFunc(i).Value, i => testFunction(i).Value);

    }
    public static RankSumResult RankSum<S>(this IEnumerable<S> items, Func<S, double> keyFunc,
      Func<S, bool> testFunction, TwoSampleHypothesis hypothesis = TwoSampleHypothesis.ValuesAreDifferent)
    {
      var ret = SegmentData(items, keyFunc, testFunction);

      return new RankSumResult(new MannWhitneyWilcoxonTest(ret.FirstSample.ToArray(), ret.SecondSample.ToArray(), hypothesis),
        ret.FirstSample.Count, ret.SecondSample.Count);
    }

    #endregion

    #region Regression

    public static LogisticRegressionImpl<T> LogisticRegression<T>(this IEnumerable<T> items, Func<T, bool?> result) 
      => new LogisticRegressionImpl<T>(items, result);
    public static LogisticRegressionImpl<T> LogisticRegression<T>(this IEnumerable<T> items, Func<T, bool> result) 
      => new LogisticRegressionImpl<T>(items, result);
    public static LinearRegressionImpl<T> LinearRegression<T>(this IEnumerable<T> items, Func<T, double?> result) 
      => new LinearRegressionImpl<T>(items, result);
    public static LinearRegressionImpl<T> LinearRegression<T>(this IEnumerable<T> items, Func<T, double> result) 
      => new LinearRegressionImpl<T>(items, result);

        #endregion

      #region Fit To Curve

    public static CurveFitter<T> Fit<T>(this IEnumerable<T> items,
      Func<T, double> xs, Func<T, double> ys, double[] guess,
      Func<double, double[], double> model)
    {
      var fitter = new CurveFitter<T>(items, xs, ys, guess, model);
      fitter.Run();
      return fitter;
    }

    #endregion

        #region Graphing
        public static CanCreateGraphWithData<T> Graph<T>(this IEnumerable<T> data, string title = "Untitled Graph") => 
      new Graph(title).WithData(data);


    #endregion

  }
}