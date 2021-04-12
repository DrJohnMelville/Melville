using System.Collections;
using System.Collections.Generic;

namespace Melville.Linq
{
    public class EnumerateEmpty<T> : IEnumerable<T>, IEnumerator<T>
    {
        public IEnumerator<T> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
        public bool MoveNext() => false;

        public void Reset()
        {
        }

        public T Current => default!;

        object IEnumerator.Current => Current!;

        public void Dispose()
        {
        }
    }
}