using System.Collections.Generic;
using System.Linq;

namespace Melville.Linq.Statistics.Tables
{
  public class GridRowOrColumn<TStorage>
  {
    public ObjectIdentitySet<TStorage> Elements { get; }
    public IList<ITableRowOrColumn<TStorage>> Headers { get; }

    public GridRowOrColumn(ObjectIdentitySet<TStorage> elements, IEnumerable<ITableRowOrColumn<TStorage>> headers)
    {
      Elements = elements;
      Headers = headers.ToList();
    }

    public IEnumerable<TStorage> CellContents(GridRowOrColumn<TStorage> other) =>
      Elements.Intersect(other.Elements);
  }
}