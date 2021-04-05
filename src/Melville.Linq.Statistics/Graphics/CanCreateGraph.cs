using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Melville.Linq.Statistics.Graphics.Internal;
using Melville.Linq.Statistics.Graphics.Internal.Axes;

namespace Melville.Linq.Statistics.Graphics
{
  public abstract class CanCreateGraph: IDumpsGraph
  {
    public abstract object ToDump();
    public abstract BitmapSource ToWpfBitmap();
    public abstract void ShowInWindow();
    public abstract T AddSerries<T>(T serries) where T:IDataSerries;

    public abstract Axis CurrentAxisX { get; }
    public abstract Axis CurrentAxisY { get; }

    public IConfigureAxis<CanCreateGraph> XAxis => new ConfigureAxis<CanCreateGraph>(CurrentAxisX, this);
    public IConfigureAxis<CanCreateGraph> YAxis => new ConfigureAxis<CanCreateGraph>(CurrentAxisY, this);
    public IConfigureAxis<CanCreateGraph> BothAxes =>MultiProxy.Create(XAxis, YAxis);

    public BarPlot<T,TLabel> Bar<T, TLabel>(IEnumerable<T> items, Func<T, double> values, Func<T, TLabel> titles) =>
      AddSerries(new BarPlot<T, TLabel>(items, values, titles));

    public BarPlot<T, string> Bar<T>(IEnumerable<T> items, Func<T, double> values) =>
      Bar(items, values, _ => string.Empty);
    public BoxPlot<T, TLabel> Box<T, TLabel>(IEnumerable<T> items, Func<T, double> values, Func<T, TLabel> titles)
      where TLabel:IComparable =>
      AddSerries(new BoxPlot<T, TLabel>(items, values, titles));

    public ScatterPlot<T> Scatter<T>(IEnumerable<T> items, Func<T, double> xFunc, Func<T, double> yFunc) =>
      AddSerries(new ScatterPlot<T>(items, xFunc, yFunc));
    public LinePlot<T> Line<T>(IEnumerable<T> items, Func<T, double> xFunc, Func<T, double> yFunc) =>
      AddSerries(new LinePlot<T>(items, xFunc, yFunc));
    public RegreesionPlot<T> Regression<T>(IEnumerable<T> items, Func<T, double> xFunc, Func<T, double> yFunc, 
      int order = 1) =>
      AddSerries(new RegreesionPlot<T>(items, xFunc, yFunc, order));
    public CanCreateGraphWithData<T> WithData<T>(IEnumerable<T> data) => AddSerries(new NullDataSerries<T>(data));

    public HistogramPlot<T> Histogram<T>(IEnumerable<T> data, Func<T, double> selector)=>
      AddSerries(new HistogramPlot<T>(data, selector));

    public abstract void WriteToDisk(string filePath, BitmapEncoder encoder, int width = -1, int height = -1);
    public void WriteToDisk(string filePath, int width = -1, int height = -1) =>
       WriteToDisk(filePath, new PngBitmapEncoder(), width, height);
  }

  public abstract class CanCreateGraphWithData<T> : CanCreateGraph
  {
    public new IConfigurAxisWithData<CanCreateGraphWithData<T>, T> XAxis => new ConfigurAxisWithData<CanCreateGraphWithData<T>,T>(CurrentAxisX, this);
    public new IConfigurAxisWithData<CanCreateGraphWithData<T>, T> YAxis => new ConfigurAxisWithData<CanCreateGraphWithData<T>, T>(CurrentAxisY, this);
    public new IConfigurAxisWithData<CanCreateGraphWithData<T>, T> BothAxes => MultiProxy.Create(XAxis, YAxis);

    public abstract IList<T> Data { get; }

    public BarPlot<T,TLabel> Bar<TLabel>(Func<T, double> values, Func<T, TLabel> titles) =>
      Bar(Data, values, titles);

    public BarPlot<T, string> Bar(Func<T, double> values) =>
      Bar(Data,values);

    public BoxPlot<T, TLabel> Box<TLabel>(Func<T, double> values, Func<T, TLabel> titles)
      where TLabel : IComparable => Box<T,TLabel>(Data, values, titles);

    public ScatterPlot<T> Scatter(Func<T, double> xFunc, Func<T, double> yFunc) =>
      Scatter(Data, xFunc, yFunc);
    public LinePlot<T> Line(Func<T, double> xFunc, Func<T, double> yFunc) =>
      Line(Data, xFunc, yFunc);

    public RegreesionPlot<T> Regression(Func<T, double> xFunc, Func<T, double> yFunc,
      int order = 1) => Regression(Data, xFunc, yFunc, order);

    public HistogramPlot<T> Histogram(Func<T, double> selector)=> Histogram(Data, selector);
  }
}