using System;
using System.Collections.Generic;
using System.Linq;

namespace Melville.Linq.Statistics.HypothesisTesting
{
  public static class UnknownFilter
  {
    public static UnknownFilter<T> Create<T>(IEnumerable<T> data) => new UnknownFilter<T>(data);
  }
  public class UnknownFilter<T>
  {
    private readonly IEnumerable<T> data;
    private readonly List<Func<T, bool>> filters = new List<Func<T, bool>>();

    public UnknownFilter(IEnumerable<T> data)
    {
      this.data = data;
    }

    public void 
      
      AddFilter<TResult>(Func<T, TResult> func) 
    {
      filters.Add(i=> func(i) != null);
    }

    public IEnumerable<T> FilteredResult()
    {
      foreach (var datum in data)
      {
        if (filters.All(i => i(datum)))
        {
          yield return datum;
        }
      }
    }
  }
}