using System;
using System.Diagnostics;
using System.Linq;

namespace Melville.CurveFit
{
  public class SimolatedAneeling:Learner
  {
    public SimolatedAneeling(Func<double[], double> goodness) : base(goodness)
    {
    }

    private double temperature = 1;

    public override double[] Run(params double[] seed)
    {
      var goodness = Goodness(seed);
      for (temperature = 1; temperature >= -0.001; temperature -= 0.05)
      {
        for (int i = 0; i < 1000; i++)
        {
          var newItem = Mutate(seed, temperature);
          var newGoodness = Goodness(newItem);
          if (Rnd.NextDouble() < temperature || newGoodness < goodness)
          {
            goodness = newGoodness;
            seed = newItem;
            Debug.WriteLine($"({string.Join(", ", seed.Select(k=>k.ToString("####0.00")))}): {goodness} | {temperature}");
          }
        }
      }
      return seed;
    }
  }
}