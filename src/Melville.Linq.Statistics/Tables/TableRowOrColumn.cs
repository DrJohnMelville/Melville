using System.Collections.Generic;

namespace Melville.Linq.Statistics.Tables
{
  public interface ITableRowOrColumn<TStorage>
  {
    string Name { get; }
    ObjectIdentitySet<TStorage> Elements { get; }
  }

  public class TableRowOrColumn<T>: ITableRowOrColumn<T>
  {
    public string Name { get; }
    public ObjectIdentitySet<T> Elements { get; }

    public TableRowOrColumn(string name, IEnumerable<T> elements)
    {
      Name = name;
      Elements = new ObjectIdentitySet<T>(elements);
    }
  }
}