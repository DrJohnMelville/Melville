using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.Graphics.Internal.Axes;

namespace Melville.Linq.Statistics.Graphics
{
  public interface IConfigureAxis<T> where T : CanCreateGraph
  {
    T IncludeValues(params double[] values);
    T Invert();
    T Log(double logBase = Math.E);
    T ShowLabels(bool show = true);
    T ExplicitLabels(params (double Value, string Display)[] values);
    T ExplicitLabels(IEnumerable<(double Value, string Display)> values);
    T LabelSize(double size);
    T LabelRotation(double size);

    T Title(string text, int? labelSize = null,
      int? labelRotation = null);

    T NewAxis();
    T SideBySide(params Action<T>[] subGraphs);
    T SideBySide(params (string title, Action<T> item)[] subGraphs);
    T WithSpecialValueGrid(double specialValue = 0.0);
    T WithSpecialValueGrid(Pen zeroPen);
    T WithSpecialValueGrid(Pen zeroPen, Pen otherPen);
    T WithSpecialValueGrid(double specialValue, Pen zeroPen);
    T WithSpecialValueGrid(double specialValue, Pen zeroPen, Pen otherPen);
    T WithGrid();
    T WithGrid(Pen pen);
    T WithGrid(Func<double, Pen> gridPen);
    
    Axis InnerAxis { get; }
  }

  public class ConfigureAxis<T>: IConfigureAxis<T> where T:CanCreateGraph
  {
    protected readonly T caller;

    public ConfigureAxis(Axis axis, T caller)
    {
      this.InnerAxis = axis;
      this.caller = caller;
    }

    public Axis InnerAxis { get; }

    #region Appearance

    public T IncludeValues(params double[] values)
    {
      foreach (var value in values)
      {
        InnerAxis.ScaledValue(value);
      }
      return caller;
    }

    public T Invert()
    {
      InnerAxis.RegisterOutputFuction((v,a)=>a.TargetRange - v, (v,a)=>a.TargetRange - v);
      return caller;
    }

    public T Log(double logBase = Math.E)
    {
      InnerAxis.RegisterInputFuction((v,a)=> 
          Math.Log(v, logBase),
        (v,a)=> Math.Pow(logBase, v) );
      InnerAxis.LogLabels = true;
      return caller;
    }
    

    #endregion

    #region Labels

    public T ShowLabels(bool show = true)
    {
      InnerAxis.ShowLabels = show;
      return caller;
    }

    public T ExplicitLabels(params (double Value, string Display)[] values) =>
      ExplicitLabels((IEnumerable<(double Value, string Display)>)values);
    public T ExplicitLabels(IEnumerable<(double Value, string Display)> values)
    {
      InnerAxis.ExplicitLabels = values.AsList();
      return caller;
    }

    public T LabelSize(double size)
    {
      InnerAxis.LabelSize = size;
      return caller;
    }
    public T LabelRotation(double size)
    {
      InnerAxis.LabelRotation = size;
      return caller;
    }

    public T Title(string text, int? labelSize = null,
      int? labelRotation = null)
    {
      caller.AddSerries(new AxisLabel<T>(new T[0], InnerAxis, text, labelSize, labelRotation));
      return caller;
    }

    #endregion

    #region Subaxes

    public T NewAxis()
    {
      InnerAxis.Parent.NewAxis();
      return caller;
    }

    public T SideBySide(params Action<T>[] subGraphs) => 
      SideBySide(subGraphs.Select(i => ("", i)).ToArray());
    public T SideBySide(params (string title, Action<T> item)[] subGraphs)
    {
      double graphWidth = 1.0 / subGraphs.Length;
      double basePosition = 0.0;
      var newGutterLevel = 0;
      foreach (var graph in subGraphs)
      {
        var axis = InnerAxis.Parent.NewAxisShareGutter();
        axis.SetOutputPlacement(graphWidth, basePosition);
        if (!string.IsNullOrWhiteSpace(graph.title))
        {
          caller.AddSerries(new AxisLabel<int>(new int[0], axis, graph.title));
        }
        graph.item(caller);
        newGutterLevel = Math.Max(newGutterLevel, axis.GutterBand);
        InnerAxis.Parent.PopAxis();
        basePosition += graphWidth;
      }
      InnerAxis.GutterBand = newGutterLevel;
      return caller;
    }
    #endregion

    #region Grid lines

    public T WithSpecialValueGrid(double specialValue = 0.0) => WithSpecialValueGrid(specialValue, Pens.Black);
    public T WithSpecialValueGrid(Pen zeroPen) => WithSpecialValueGrid(0.0, zeroPen, Pens.LightGray);
    public T WithSpecialValueGrid(Pen zeroPen, Pen otherPen) => WithSpecialValueGrid(0.0, zeroPen, otherPen);

    public T WithSpecialValueGrid(double specialValue, Pen zeroPen) => WithSpecialValueGrid(specialValue, zeroPen,Pens.LightGray);
    public T WithSpecialValueGrid(double specialValue, Pen zeroPen, Pen otherPen) => 
      WithGrid(d=> Math.Abs(d-specialValue) <(100*double.Epsilon)?zeroPen:otherPen);
    public T WithGrid() => WithGrid(Pens.Black);
    public T WithGrid(Pen pen) => WithGrid(_ => pen);
    public T WithGrid(Func<double, Pen> gridPen)
    {
      InnerAxis.GridPen = gridPen;
      return caller;
    }
    #endregion
  }

  public interface IConfigurAxisWithData<T, TData>: IConfigureAxis<T> where T:CanCreateGraph
  {
    public T WhereSideBySide<TKey>(Func<TData, TKey> selector, Action<T> subGraph);
  }
  public class ConfigurAxisWithData<T, TData> : ConfigureAxis<T>, IConfigurAxisWithData<T, TData> where 
    T : CanCreateGraphWithData<TData>
  {
    public ConfigurAxisWithData(Axis axis, T caller) : base(axis, caller)
    {
    }

    public T WhereSideBySide<TKey>(Func<TData, TKey> selector, Action<T> subGraph)
    {
      var groups = caller.Data.GroupBy(selector).OrderBy(i => i.Key)
       .Select(i=> (i.Key.ToString(), (Action<T>)((T j)=>subGraph((T)j.WithData(i)))));
      return SideBySide(groups.ToArray());
    }
  }
}