using System;
using System.Collections;
using System.Collections.Generic;

namespace Melville.Linq
{
    public class RecursiveSelector<T> : IEnumerable<T>
    {
        private static readonly EnumerateEmpty<T> enumerateEmpty = new ();
        private readonly IEnumerable<T> basis;
        private readonly Func<T, IEnumerable<T>?> children;

        public RecursiveSelector(IEnumerable<T> basis, Func<T, IEnumerable<T>?> children)
        {
            this.basis = basis;
            this.children = children;
        }

        public IEnumerator<T> GetEnumerator() =>
            new RecursiveEnumerator(basis.GetEnumerator(),
                i => children(i)?.GetEnumerator() ?? enumerateEmpty);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class RecursiveEnumerator : IEnumerator<T>
        {
            private readonly Stack<IEnumerator<T>> context = new();
            private readonly Func<T, IEnumerator<T>> selector;

            
            public RecursiveEnumerator(IEnumerator<T> firstEnumerator, Func<T, IEnumerator<T>> selector)
            {
                this.selector = selector;
                Current = default!;
                context.Push(firstEnumerator);
            }

            public bool MoveNext()
            {
                while (context.TryPeek(out var top))
                {
                    if (top.MoveNext())
                    {
                        SetCurrentAndPushChildren(top.Current);
                        return true;
                    }
                    PopCompletedEnumerator();
                }

                return false;
            }

            private void PopCompletedEnumerator() => context.Pop().Dispose();

            private void SetCurrentAndPushChildren(T nextValue)
            {
                Current = nextValue!;
                context.Push(selector(nextValue));
            }

            public void Reset() => throw new NotSupportedException();

            public T Current { get; private set; }

            object IEnumerator.Current => Current!;

            public void Dispose()
            {
                while (context.TryPop(out var item))
                {
                    item.Dispose();
                }
            }
        }
    }
}