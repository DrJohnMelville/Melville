using System;
using System.Collections.Generic;

namespace Melville.Lists.AdvancedLists.PersistentLinq
{
  public static class LinqCollectionOperations
  {
    public static WhereCollection<T> WhereCol<T>(this IList<T> list, Func<T, bool> predicate)
    {
      return new WhereCollection<T>(list, predicate);
    }

    public static SelectCollection<TSource, TDest> SelectCol<TSource, TDest>(this IList<TSource> list,
      Func<TSource, TDest> sel) where TSource:notnull
    {
      return new SelectCollection<TSource, TDest>(list, sel);
    }
    public static SelectManyCollection<TSource, TDest> SelectManyCol<TSource, TDest>(this IList<TSource> list,
      Func<TSource, IList<TDest>> sel)
    {
      return new SelectManyCollection<TSource, TDest>(list, sel);
    }
  }
}