using System.Collections;
using System.Collections.Generic;

namespace Melville.Linq
{
    public class EnumerateSingle<T> : IEnumerable<T>
    {
        private T value;

        public EnumerateSingle(T value)
        {
            this.value = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SingleEnumerator(value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private class SingleEnumerator: IEnumerator<T>
        {
            public T Current { get; }
            object IEnumerator.Current => Current!;
            private byte state = 0;

            public SingleEnumerator(T value)
            {
                Current = value;
            }

            public bool MoveNext() => (++state) == 1;

            public void Reset()
            {
                state = 0;
            }


            public void Dispose()
            {
            }
        }
    }
}