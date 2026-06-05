using Melville.INPC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Melville.Wpf.Samples.KeyCounters;

public partial class KeyCount(string name)
{
    public string Name { get; } = name;

    [AutoNotify] public partial int Count { get; private set; }
    public void Increment() => Count++;
}

[AutoNotify]
internal partial class KeyCounterViewModel
{
    private readonly ConcurrentDictionary<Key, KeyCount> counts = new();
    public IEnumerable<KeyCount> Counts => counts.Values;

    public void KeyPress(KeyEventArgs args)
    {
        counts.GetOrAdd(args.Key, i =>
        {
            return new KeyCount(i.ToString());
        })?.Increment();
        CollectionUpdated();
    }

    private void CollectionUpdated()
    {
        ((IExternalNotifyPropertyChanged)this).NotifyPropertyChange(nameof(Counts));
    }

    public void Reset()
    {
        counts.Clear();
        CollectionUpdated();
    }
}
