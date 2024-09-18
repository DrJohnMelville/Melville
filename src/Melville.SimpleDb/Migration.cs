using System;
using Melville.INPC;

namespace Melville.SimpleDb;

public partial class Migration : IComparable
{
    [FromConstructor] public int Version { get; }
    [FromConstructor] public string UpgradeScript { get; }

    public int CompareTo(object? obj) => obj is Migration m ? Version.CompareTo(m.Version) : 0;
}