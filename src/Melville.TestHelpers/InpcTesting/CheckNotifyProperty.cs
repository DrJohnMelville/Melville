using  System;
using System.ComponentModel;
using System.Linq.Expressions;

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
