using System.Collections.Generic;
using System.Linq;
using Melville.Linq.Statistics.Graphics.Gutters;
using Melville.Linq.Statistics.Graphics.Internal;
using Melville.Linq.Statistics.Graphics.Internal.Axes;

namespace Melville.Linq.Statistics.Graphics
{
  public sealed partial class Graph: CanCreateGraph
  {
    public IList<IDataSerries> Data { get; } = new List<IDataSerries>();


    public override T AddSerries<T>(T serries)
    {
      serries.GraphHost = this;
      serries.SetAxes(CurrentAxisX, CurrentAxisY);
      if (! (serries.TransientSerries))
      {
        Data.Add(serries);
      }
      return serries;
    }

    #region Rendering
    public void Render(ILowLevelGraphSurface surface)
    {
      if (delayAxisNotification)
      {
        foreach (var axis in AllAxes())
        {
          axis.Gutter.Freeze();
        }
      }
      MeasureSurface();
      RenderToSurface(new GraphSurface(surface));
    }

    private void RenderToSurface(GraphSurface surface)
    {
      RenderGutters(surface);
      foreach (var datum in Data)
      {
        surface.SetAxes(datum.CurrentAxisX, datum.CurrentAxisY);
        datum.Render(surface);
      }
    }

    private void RenderGutters(GraphSurface surface)
    {
      surface.SetAxes(xAxes.Axes.First(), yAxes.Axes.First());
      if (surface.XAxis.Gutter.IsFrozen) return;
      var allAxes = AllAxes();
      foreach (var axis in allAxes)
      {
        axis.Gutter.Clear();
        axis.Gutter.Add(new GutterText(0,"W",0, 0.1, int.MaxValue)); // putting anything in every gutter makes the size rrecalculate correctly
      }
      foreach (var axis in allAxes)
      {
        axis.RenderLabelsAndLines(surface);
      }
      
    }

    private IEnumerable<Axis> AllAxes() => Data.SelectMany(i => new[] {i.CurrentAxisX, i.CurrentAxisY}).Distinct();

    private void MeasureSurface()
    {
      RenderToSurface(new GraphSurface(new VoidSurface()));
    }
    public void SetActualExtent(double width, double height)
    {
      foreach (var dataSerriese in Data)
      {
        dataSerriese.CurrentAxisX.TargetRange = width;
        dataSerriese.CurrentAxisY.TargetRange = height;
      }
    }
    #endregion

    #region Colors
    private bool colorsAssigned;

    public void AssignColors()
    {
      if (colorsAssigned) return;
      colorsAssigned = true;
      var assigner = new ColorAssigner();
      foreach (var datum in Data)
      {
        datum.AssignColors(assigner);
      }
    }
    #endregion

  }
}