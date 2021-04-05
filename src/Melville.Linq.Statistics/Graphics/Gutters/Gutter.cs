using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Melville.Linq.Statistics.Graphics.Gutters
{
  public class Gutter
  {
    public ICollection<GutterItem> Items { get; } = new ObservableCollection<GutterItem>();

  
    public void Add(GutterItem g)
    {
      if (!IsFrozen)
      {
        Items.Add(g);
      }
    }

    public void Clear()
    {
      if (!IsFrozen)
      {
        Items.Clear();
      }
    }

    public bool IsFrozen { get; private set; } = false;
    public void Freeze() => IsFrozen = true;
  }

  public class GutterItem
  {
    public double Offset { get; }
    public int Level { get; }

    public GutterItem(double offset, int level)
    {
      Offset = offset;
      Level = level;
    }
  }

  public class GutterText : GutterItem
  {
    public string Text { get; }
    public double Rotation { get; }
    public double TextSize { get; }

    public GutterText(double offset, string text, double rotation, double textSize, int level) : base(offset, level)
    {
      Text = text;
      Rotation = rotation;
      TextSize = textSize;
    }
  }
}