using System.Collections;
using System.Collections.Generic;
using Melville.Linq.Statistics.Functional;

namespace Melville.Linq.Statistics.Tables
{
  public class ObjectIdentitySet<T>: IEnumerable<T>
  {
    private readonly IList<T> items;
    public ObjectIdentitySet() : this (new List<T>())
    {
    }

    public ObjectIdentitySet(IEnumerable<T> collection)
    {
      items = collection.AsList();
    }
      
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => items.Count;

    public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

    public IEnumerable<T> Intersect(ObjectIdentitySet<T> other)
    {
      foreach (var item in items)
      {
        foreach (var otherItem in other.items)
        {
          if (object.ReferenceEquals(item, otherItem))
          {
            yield return item;
          }
        }
      }
    }
  }
  /*  public class ObjectIdentitySet<T> : HashSet<T>
    {
      public ObjectIdentitySet() : base(EqualityComparer.Singleton)
      {
      }

      public ObjectIdentitySet(IEnumerable<T> collection) : base(collection, EqualityComparer.Singleton)
      {
      }

      private class EqualityComparer : IEqualityComparer<T>
      {
        public static readonly IEqualityComparer<T> Singleton = new EqualityComparer();

        private EqualityComparer()
        {
        }

        public bool Equals(T x, T y)
        {
          return ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
          return RuntimeHelpers.GetHashCode(obj);
        }
      }
    }*/
}