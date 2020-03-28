using System;
using System.Linq;

namespace Melville.CurveFit
{
  public class CompositeLearner : Learner
  {
    public readonly Learner[] learners;

    public CompositeLearner(Func<double[], double> goodness, params Func<Func<double[],double>, Learner>[] factories) : base(goodness)
    {
      learners = factories.Select(i => i(goodness)).ToArray();
    }

    public override double[] Run(params double[] seed)
    {
      foreach (var learner in learners)
      {
        seed = learner.Run(seed);
      }
      return seed;
    }
  }
}