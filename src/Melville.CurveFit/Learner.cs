using System;

namespace Melville.CurveFit
{
  public abstract class Learner
  {
    protected readonly Random Rnd = new Random();

    protected readonly Func<double[], double> Goodness;

    protected Learner(Func<double[], double> goodness)
    {
      this.Goodness = goodness;
    }

    public abstract double[] Run(params double[] seed);

    protected double[] Replace(double[] current, int index, double replacementValue)
    {
      var ret = new double[current.Length];
      for (int i = 0; i < ret.Length; i++)
      {
        ret[i] = i == index ? replacementValue : current[i];
      }
      return ret;
    }

    protected double[] Mutate(double[] seed, double range) => Mutate(seed, i => range);
    protected double[] Mutate(double[] seed, Func<int,double> range)
    {
      var which = Rnd.Next(seed.Length);
      return Replace(seed, which, MutateValue(seed[which], range(which)));
    }

    private double MutateValue(double source, double range)
    {
      var factor = (Rnd.NextDouble()*range * 2) - range;
      return Math.Abs(source) < 2* double.Epsilon? factor: source + factor;
    }

    public void Crossover(double[] a, double[] b, out double[] child1, out double[] child2 )
    {
      int len = a.Length;
      int crossPoint = Rnd.Next(a.Length - 2) + 1;
      child1 = new double[len];
      child2 = new double[len];
      for (int i = 0; i < crossPoint; i++)
      {
        child1[i] = a[i];
        child2[i] = b[i];
      }
      for (int i = crossPoint; i < len; i++)
      {
        child1[i] = b[i];
        child2[i] = a[i];
      }
    }
  }
}