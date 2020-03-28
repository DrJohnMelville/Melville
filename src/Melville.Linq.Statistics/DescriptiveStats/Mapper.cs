using System;
using System.Collections.Generic;

namespace Melville.Linq.Statistics.DescriptiveStats
{
  public class Mapper<T>: Mapper<T,T> where T : IComparable<T>
  {
    public Mapper() : this(i => i) { }
    public Mapper(Func<T, T> defaultFunc) : base(defaultFunc){}

  }
  public class Mapper<TIn, TOut> where TIn:IComparable<TIn>
  {
    private Dictionary<TIn, TOut> mappings = new Dictionary<TIn, TOut>();
    private readonly Func<TIn, TOut> defaultFunc;

    public Mapper(Func<TIn, TOut> defaultFunc)
    {
      this.defaultFunc = defaultFunc;
    }

    public void SetMapping(TOut result, params TIn[] synonyms) =>
      SetMapping(result, (IEnumerable<TIn>) synonyms);

    public void SetMapping(TOut result, IEnumerable<TIn> synonyms)
    {
      foreach (var synonym in synonyms)
      {
        mappings[synonym] = result;
      }
    }
    public TOut Map(TIn input) => mappings.TryGetValue(input, out var ret) ? ret : defaultFunc(input);
  }
}