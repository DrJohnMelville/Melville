using System;

namespace Melville.CurveFit
{


  public class HillClimber: Learner
  {
    public double Epsilon { get; set; } = 0.0001;
    public double InitialDelta { get; set; } = 10;
    public HillClimber(Func<double[], double> goodness) : base(goodness)
    {
    }

    public override double[] Run(params double[] seed)
    {
      var current = seed;
      double[] starting;
      do
      {
        starting = current;
        for (int i = 0; i < current.Length; i++)
        {
          current = ClimbHill(current, i);
        }
      } while (current != starting);
      return current;
    }

    private double[] ClimbHill(double[] current, int index)
    {
      for (double delta = InitialDelta; delta > Epsilon; delta /= 10)
      {
        current = ClimbHill(current, index, delta);
      }
      return current;
    }

    private double[] ClimbHill(double[] current, int index, double delta)
    {
      var initialGoodness = Goodness(current);
      var down = Replace(current, index, current[index] - delta);
      var downGoodnenss = Goodness(down);
      var up = Replace(current, index, current[index] + delta);
      var upGoodness = Goodness(up);

      if (initialGoodness <= downGoodnenss && initialGoodness <= upGoodness) // already optimal
      {
        return current;
      }
      return upGoodness < downGoodnenss ? RollDownHill(up, index, delta, upGoodness) : RollDownHill(down, index, -delta, downGoodnenss);
    }

    private double[] RollDownHill(double[] current, int index, double delta, double goodness)
    {
      while (true)
      {
        var candidate = Replace(current, index, current[index] + delta);
        var candidateGoodness = Goodness(candidate);
        if (candidateGoodness > goodness) return current;
        current = candidate;
        goodness = candidateGoodness;
      }
    }
  }
}