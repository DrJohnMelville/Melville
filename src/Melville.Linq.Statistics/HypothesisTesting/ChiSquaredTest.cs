using System;
using System.Collections.Generic;
using Melville.Linq.Statistics.Tables;

namespace Melville.Linq.Statistics.HypothesisTesting
{
  public static class ChiSquaredTest
  {
    public static ChiSquaredStatisic ChiSquared<TD, TP1, TP2>(this IEnumerable<TD> data,
      Func<TD, TP1> rows, Func<TD, TP2> cols, RequireClass<TD> reserved = null) where TD:class =>
      data.Table().WithRows("", rows).WithColumns("", cols).ChiSquared();
    public static ChiSquaredStatisic ChiSquared<TD, TP1, TP2>(this IEnumerable<TD> data,
      Func<TD, TP1> rows, Func<TD, TP2> cols, RequireStruct<TD> reserved = null) where TD:struct =>
      data.Table().WithRows("", rows).WithColumns("", cols).ChiSquared();
  }
}