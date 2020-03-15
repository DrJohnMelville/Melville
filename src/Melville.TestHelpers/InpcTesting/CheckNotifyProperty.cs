using  System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xunit;

namespace Melville.TestHelpers.InpcTesting
{
  public static class CheckNotifyProperty
  {
    public static void AssertProperty<TParent, TProperty>(this TParent parent,
      Expression<Func<TParent, TProperty>> accessor, TProperty newValue,
      params Expression<Func<TParent, object>>[] otherProperties) where
      TParent : INotifyPropertyChanged
    {
      PropertyTestRecord.Create(accessor, newValue, parent, otherProperties).RunINPCCheck(parent);
    }
  }
}
