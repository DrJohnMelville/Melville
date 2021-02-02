using System;
using System.Collections.Generic;
using System.Linq;

namespace Melville.Generators.INPC.CodeWriters
{
    public class CompositeDispose : IDisposable
    {
        public static IDisposable DisposeInReverseOrder(IList<IDisposable> candidates)
        {
            if (candidates.Count == 1) return candidates[0];
            return new CompositeDispose(candidates.Reverse());
        }
        private readonly IEnumerable<IDisposable> items;

        public CompositeDispose(IEnumerable<IDisposable> items)
        {
            this.items = items;
        }

        public void Dispose()
        {
            foreach (var item in items)
            {
                item.Dispose();
            }
        }
    }
}