using System;
using System.Collections.Generic;
using System.Linq;
using Melville.CurveFit.MpFit;

namespace Melville.CurveFit
{


  public class CurveFitter<T>
  {
    public IList<T> Data { get; }
    public Func<T, double> XFunc { get; }
    public Func<T, double> YFunc { get; }
    public Func<double, double[], double> Model { get; }
    public double[] Solution { get; }

    public CurveFitter(IEnumerable<T> data, Func<T, double> xFunc, Func<T, double> yFunc, double[] initialSolution, Func<double, double[], double> model)
    {
      Data = data.ToList();
      XFunc = xFunc;
      YFunc = yFunc;
      Model = model;
      Solution = initialSolution;
    }

    public void Run()
    {
        var result = new MpResult(Solution.Length);
        MpFit.MpFit.Solve(ComputeResiduals, Data.Count, Solution.Length, Solution,
            null, null, null, ref result);
    }

      /* 
        * quadratic fit function
        *
        * p - array of fit parameters 
        * dy - array of residuals to be returned
        * CustomUserVariable - private data (struct vars_struct *)
        *
        * RETURNS: error code (0 = success)
        */
      public int ComputeResiduals(double[] p, double[] dy, IList<double>[] dvec, object vars)
      {
          for (int i = 0; i < dy.Length; i++)
          {
              dy[i] = YFunc(Data[i]) - Model(XFunc(Data[i]), p);
          }
          return 0;
      }

      public double EvaluateAt(double x) => Model(x, Solution);
  }


}