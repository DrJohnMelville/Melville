using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Accord.Statistics.Analysis;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.Tables;

namespace Melville.Linq.Statistics.HypothesisTesting
{
  public abstract class RegressionBase<T, TResult, TFinalType> where TResult:struct
  {
    protected Func<T, TResult> resultsFunc { get; }
    private readonly IList<T> rawItems;


    protected RegressionBase(IEnumerable<T> items, Func<T, TResult> resultsFunc)
    {
      rawItems = items.AsList();
      Filter = new UnknownFilter<T>(rawItems);
      this.resultsFunc = resultsFunc;
    }

    protected RegressionBase(IEnumerable<T> items, Func<T, TResult?> resultFunc) :
      this(items, i => resultFunc(i).Value)
    {
      Filter.AddFilter(resultFunc);
    }

    protected IList<RegressionVariableDecl> IndependantVariables { get; } = new List<RegressionVariableDecl>();

    protected UnknownFilter<T> Filter { get; }

    protected double[][] InputData()
    {
      return Filter.FilteredResult().Select(i => IndependantVariables.Select(j => j.ValueFunc(i)).ToArray()).ToArray();
    }


    #region VariableMethods

    private TFinalType ReturnValue => (TFinalType)(object) this;

    public TFinalType WithVariable(Expression<Func<T, double?>> selector) => WithVariable(
      ExpressionPrinter.Print(selector), selector.Compile());
    public TFinalType WithVariable(string name, Func<T, double?> selector)
    {
      Filter.AddFilter(selector);
      return WithVariable(name, i => selector(i).Value);
    }
    public TFinalType WithVariable(Expression<Func<T, double>> selector) => 
      WithVariable(ExpressionPrinter.Print(selector), selector.Compile());
    public TFinalType WithVariable(string name, Func<T, double> selector)
    {
      IndependantVariables.Add(new RegressionVariableDecl(name, selector));
      return ReturnValue;
    }

    public TFinalType WithVariable(Expression<Func<T, bool>> selector) =>
      WithVariable(ExpressionPrinter.Print(selector), selector.Compile());
    public TFinalType WithVariable(string name, Func<T, bool> selector) => 
      WithVariable(name, i=>selector(i)?1.0:0.0);

    public TFinalType WithVariable(Expression<Func<T, bool?>> selector) =>
      WithVariable(ExpressionPrinter.Print(selector), selector.Compile());
    public TFinalType WithVariable(string name, Func<T, bool?> selector)
    {
      Filter.AddFilter(selector);
      return WithVariable(name, i => selector(i).Value);
    }

    public TFinalType WithDummyVariable<TVariable>(Expression<Func<T, TVariable?>> selector,
      TVariable? referrant = null) where TVariable : struct =>
      WithDummyVariable(ExpressionPrinter.Print(selector), selector.Compile(), referrant);
    public TFinalType WithDummyVariable<TVariable>(string name, Func<T, TVariable?> selector, TVariable? referrant = null) where TVariable : struct
    {
      Filter.AddFilter(selector);
      return InnerWithDummyVariable(name, selector, referrant);
    }

    public TFinalType WithDummyVariable<TVariable>(Expression<Func<T, TVariable>> selector, TVariable? referrant = null,
      RequireStruct<TVariable> reserved = null) where TVariable : struct =>
      WithDummyVariable(ExpressionPrinter.Print(selector), selector.Compile(), referrant, reserved);
    public TFinalType WithDummyVariable<TVariable>(string name, Func<T, TVariable> selector, TVariable? referrant = null, RequireStruct<TVariable> reserved = null) where TVariable : struct =>
      InnerWithDummyVariable(name, selector, referrant);

    public TFinalType WithDummyVariable<TVariable>(Expression<Func<T, TVariable>> selector, TVariable referrant = null,
      RequireClass<TVariable> reserved = null) where TVariable : class =>
      WithDummyVariable(ExpressionPrinter.Print(selector), selector.Compile(), referrant, reserved);
    public TFinalType WithDummyVariable<TVariable>(string name, Func<T, TVariable> selector, TVariable referrant = null, RequireClass<TVariable> reserved = null) where TVariable : class
    {
      Filter.AddFilter(selector);
      return InnerWithDummyVariable(name, selector, referrant);
    }

    private TFinalType InnerWithDummyVariable<TVariable>(string name, Func<T, TVariable> selector, object referrant)
    {
      var values = rawItems.Select(selector).Where(i => i != null).Distinct().OrderBy(i => i);
      referrant = referrant ?? values.FirstOrDefault();
      SetupDummyVariable(name, selector, values.Where(i=>!i.Equals(referrant)));
      return ReturnValue;
    }

    private void SetupDummyVariable<TVariable>(string name, Func<T, TVariable> selector, IEnumerable<TVariable> values)
    {
      foreach (var value in values)
      {
        var capturedValue = value;
        WithVariable($"{name}: {value}", i => selector(i).Equals(capturedValue) ? 1.0 : 0.0);
      }
    }

    #endregion

    #region RegressionVariableDecl

    public class RegressionVariableDecl 
    {
      public string Name { get; }
      public Func<T, double> ValueFunc {get;}

      public RegressionVariableDecl(string name, Func<T, double> valueFunc)
      {
        Name = name;
        ValueFunc = valueFunc;
      }
    }
    #endregion

    protected string[] GetInputNames() => IndependantVariables.Select(i=>i.Name).ToArray();
  }


  public sealed class 
        LogisticRegressionImpl<T>:RegressionBase<T, bool, LogisticRegressionImpl<T>>
  {
    public LogisticRegressionImpl(IEnumerable<T> items, Func<T, bool> resultsFunc) : base(items, resultsFunc)
    {
    }

    public LogisticRegressionImpl(IEnumerable<T> items, Func<T, bool?> resultFunc) : base(items, resultFunc)
    {
    }

    public LogisticRegressionAnalysis Regress()
    {
      var analyzer = new LogisticRegressionAnalysis()
      {
        Iterations = 100,
        Inputs = GetInputNames(),
      };
      analyzer.Learn(InputData(), Filter.FilteredResult().Select(i=>resultsFunc(i)?1.0:0.0).ToArray());
      return analyzer;
    }

    private object ToDump()
    {
      var result = Regress();
      return new XElement("LinqPad.Html",
        new XElement("div",
          new XElement("table",
            new XElement("tr", new XElement("td", "N"), new XElement("td", result.NumberOfSamples)),
            new XElement("tr", new XElement("td", "Chi Square"), new XElement("td", result.ChiSquare.Statistic)),
            new XElement("tr", new XElement("td", "Degrees of Freedom"), new XElement("td", result.ChiSquare.DegreesOfFreedom)),
            new XElement("tr", new XElement("td", "P Value"), new XElement("td", result.ChiSquare.PValue.ToString("0.0000")))
            ),
            new XElement("table",
            new XElement("tr", new XElement("th","Name"),new XElement("th","Odds Ratio"),new XElement("th","P Value"),
            new XElement("th","Confidence Interval")),
              result.Coefficients.Skip(1).Select(i=>new XElement("tr", new XElement("td", i.Name), new XElement("td", i.OddsRatio.ToString("####0.##")),
              new XElement("td", i.Wald.PValue.ToString("0.0000")), 
              new XElement("td", $"{i.ConfidenceLower:###0.##} - {i.ConfidenceUpper:###0.##}"))
            )
            )));
    }
  }

  public sealed class LinearRegressionImpl<T> : RegressionBase<T, double, LinearRegressionImpl<T>>
  {
    public LinearRegressionImpl(IEnumerable<T> items, Func<T, double> resultsFunc) : base(items, resultsFunc)
    {
    }

    public LinearRegressionImpl(IEnumerable<T> items, Func<T, double?> resultFunc) : base(items, resultFunc)
    {
    }

    public MultipleLinearRegressionAnalysis Regress()
    {
      var analyzer = new MultipleLinearRegressionAnalysis(true)
      {
        Inputs = GetInputNames()
      };
      analyzer.Learn(InputData(), Filter.FilteredResult().Select(resultsFunc).ToArray());
      return analyzer;
    }

    private object ToDump()
    {
      var result = Regress();
      return new XElement("LinqPad.Html",
        new XElement("div",
          new XElement("table",
            new XElement("tr", new XElement("td", "N"), new XElement("td", result.NumberOfSamples)),
            new XElement("tr", new XElement("td", "Adjusted R\x00B2"), new XElement("td", result.RSquareAdjusted.ToString("0.0000"))),
            new XElement("tr", new XElement("td", "F Test (p)"), new XElement("td", result.FTest.Statistic.ToString("0.0000")), new XElement("td", result.FTest.PValue.ToString("0.0000"))),
            new XElement("tr", new XElement("td", "Z Test (p)"), new XElement("td", result.ZTest.Statistic.ToString("0.0000")), new XElement("td", result.ZTest.PValue.ToString("0.0000"))),
            new XElement("tr", new XElement("td", "Chi Squared Test (p)"), new XElement("td", result.ChiSquareTest.Statistic.ToString("0.0000")), new XElement("td", result.ChiSquareTest.PValue.ToString("0.0000")))
//            new XElement("tr", new XElement("td", "Chi Square"), new XElement("td", result.ChiSquare.Statistic)),
//            new XElement("tr", new XElement("td", "Degrees of Freedom"), new XElement("td", result.ChiSquare.DegreesOfFreedom)),
//            new XElement("tr", new XElement("td", "P Value"), new XElement("td", result.ChiSquare.PValue.ToString("0.0000")))
            ),
            new XElement("table",
            new XElement("tr", new XElement("th","Name"),new XElement("th","Coefficient"),new XElement("th","T Statistic"),new XElement("th","P Value"),
            new XElement("th","Confidence Interval")),
              result.Coefficients.Select(i=>new XElement("tr", new XElement("td", i.Name), new XElement("td", i.Value.ToString("####0.##")),
              new XElement("td", i.TTest.Statistic.ToString("0.0000")), new XElement("td", i.TTest.PValue.ToString("0.0000")), 
              new XElement("td", $"{i.ConfidenceLower:###0.##} - {i.ConfidenceUpper:###0.##}"))
            )
            )));
    }
  }
}