using System;
using System.Linq;
using Xunit;

namespace Melville.CurveFit.CurveFitTest
{
    public class SimpleFittingTests
    {
      [Fact]
      public void ConstantFunction()
      {
        var answer = new HillClimber(i => Math.Pow(5 - i[0], 2)).Run(0);
        Assert.Equal(5.0, answer[0]);

      }

      [Fact]
      public void ElementCombination3()
      {
        var answer = new HillClimber(i=> Math.Pow((i[0]-1000.21),2) + Math.Pow((i[1] + 13),2) + Math.Pow(i[2], 2)).Run(0,0,0);
        Assert.Equal(1000.21, answer[0], 2);
        Assert.Equal(-13, answer[1], 2);
        Assert.Equal(0, answer[2], 2);
      }

      [Fact] public void FindParabola()
      {
        var fitter = MakeFitter((x) => -2.5 * x + 15 * x * x + Math.PI*x*x*x, new double[3],
          (x, v) => (v[0] * x) + (v[1] * x * x) + (v[2] * x * x * x), -10, 0, 20);
        fitter.Run();
        Assert.Equal(-2.5, fitter.Solution[0], 2);
        Assert.Equal(15, fitter.Solution[1], 3);
        Assert.Equal(Math.PI, fitter.Solution[2], 4);
      }


    public static CurveFitter<double> MakeFitter(Func<double, double> funcToFit,
      double[] initialSolution, Func<double, double[], double> model,
      double xmin, double xmax, int points)
    {
      double delta = (xmax - xmin) / points;
      var xs = Enumerable.Range(0, points).Select(i => xmin + (i * delta)).ToArray();
      return new CurveFitter<double>(xs, i => i, funcToFit, initialSolution, model);
    }

  }
}
