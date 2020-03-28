using System.Collections.Generic;
using System.Linq;
using Melville.Linq.Statistics.Functional;

namespace Melville.Linq.Statistics.Tables
{
  public interface ITableAxis<TStorage>
  {
    string Name { get; }
    IList<ITableRowOrColumn<TStorage>> Elements { get; }
  }
  public class TableAxis<TStorage>: ITableAxis<TStorage>
  {
    public string Name { get; }
    public IList<ITableRowOrColumn<TStorage>> Elements { get; }

    public TableAxis(string name, IEnumerable<ITableRowOrColumn<TStorage>> elements)
    {
      Name = name;
      Elements = elements.AsList();
    }
  }

  public static class AxisList
  {
    public static int DimensionLength<TStorage>(this IList<ITableAxis<TStorage>> axis, int start)
    {
      int ret = 1;
      for (int i = start + 1; i < axis.Count(); i++)
      {
        ret *= axis[i].Elements.Count;
      }
      return ret;
    }
    public static int DimensionCopies<TStorage>(this IList<ITableAxis<TStorage>> axis, int start)
    {
      int ret = 1;
      for (int i = 0; i < axis.Count() && i < start; i++)
      {
        ret *= axis[i].Elements.Count;
      }
      return ret;
    }

  }
}