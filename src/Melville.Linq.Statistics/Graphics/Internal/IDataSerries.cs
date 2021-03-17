using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.Graphics.Internal.Axes;

namespace Melville.Linq.Statistics.Graphics.Internal
{
  public interface IDataSerries: IDumpsGraph
  {
    void Render(GraphSurface surface);
    void AssignColors(ColorAssigner assigner);
    CanCreateGraph GraphHost { get; set; }
    void SetAxes(Axis xAxis, Axis yAxis);
    Axis CurrentAxisX { get; }
    Axis CurrentAxisY { get; }
    bool TransientSerries { get; }
  }

  public abstract class XYDataSerries<T, TReturn> : YDataSerries<T, TReturn>
    where TReturn : XYDataSerries<T, TReturn>
  {
    public Func<T, double> XFunc { get; }

    protected XYDataSerries(IEnumerable<T> data, Func<T, double> xFunc, Func<T, double> yFunc) : base(data, yFunc)
    {
      XFunc = xFunc;
    }
  }

  public abstract class YDataSerries<T, TReturn> : DataSerries<T, TReturn>
    where TReturn : YDataSerries<T, TReturn>
  {
    public Func<T, double> YFunc { get; }

    protected YDataSerries(IEnumerable<T> data, Func<T, double> yFunc) : 
      base(data)
    {
      YFunc = yFunc;
    }
  }

  public class NullDataSerries<T> : DataSerries<T, NullDataSerries<T>>
  {
    public NullDataSerries(IEnumerable<T> data) : base(data)
    {
    }

    #region Overrides of DataSerries<T,NullDataSerries<T>>

    public override void Render(GraphSurface surface)
    { 
    }

    #region Overrides of DataSerries<T,NullDataSerries<T>>

    public override bool TransientSerries => true;

    #endregion

    #endregion
  };
  public abstract class DataSerries<T, TReturn> : CanCreateGraphWithData<T>, IDataSerries
    where TReturn:DataSerries<T, TReturn>
  {
    public override IList<T> Data { get; }
    public Func<T, Pen> Pen { get; set; }
    public Func<T, Brush> Brush { get; set; }

    #region Implementation of IDataSerries

    public virtual bool TransientSerries => false;

    #endregion

    protected DataSerries(IEnumerable<T> data)
    {
      Data = data.AsList();
    }

    #region Set Brush and Pen

    public TReturn WithBrush(Brush brush) => WithBrush(_ => brush);
    public TReturn WithBrush(Func<T, Brush> brush)
    {
      this.Brush = brush;
      return (TReturn)this;
    }

    public TReturn WithPen(Pen pen) => WithPen(_ => pen);
    public TReturn WithPen(Func<T, Pen> pen)
    {
      this.Pen = pen;
      return (TReturn)this;
    }

    #endregion

    #region Rendering

    public abstract void Render(GraphSurface surface);

    public void AssignColors(ColorAssigner assigner)
    {
      if (Pen != null && Brush != null) return;
      var color = assigner.Color();
      var br = new SolidColorBrush(color);
      var pen = new Pen(br, 1);
      Pen = Pen ?? (_ => pen);
      Brush = Brush ?? (_ => br);
    }

    #endregion

    #region Dump the graph instead of this
    public CanCreateGraph GraphHost { get; set; }

    public void SetAxes(Axis xAxis, Axis yAxis)
    {
      SetAxesProtected(xAxis, yAxis);
    }

    protected virtual void SetAxesProtected(Axis xAxis, Axis yAxis)
    {
      innerXAxis = xAxis;
      innerYAxis = yAxis;
    }
    public override object ToDump() => GraphHost.ToDump();
    public override BitmapSource ToWpfBitmap() => GraphHost.ToWpfBitmap();

    public override void WriteToDisk(string filePath, BitmapEncoder encoder, int width, int height = -1) =>
      GraphHost.WriteToDisk(filePath, encoder, width, height);
    
    public override void ShowInWindow() => GraphHost.ShowInWindow();

    public override TNewSerries AddSerries<TNewSerries>(TNewSerries serries) => GraphHost.AddSerries(serries);
    private Axis innerXAxis;
    public override Axis CurrentAxisX => innerXAxis;
    private Axis innerYAxis;
    public override Axis CurrentAxisY => innerYAxis;
    #endregion
  }

}