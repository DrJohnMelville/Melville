using  System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.CSharpHacks;

namespace Melville.MVVM.Wpf.ViewFrames
{
  public sealed class SelectViewModel<TSource, TDest>:NotifyBase, ICreateView where TSource:INotifyPropertyChanged
  {
    private TSource source;
    private Func<TSource, TDest> selectFunc;
    public TDest Value => selectFunc(source);
    public SelectViewModel(TSource source, Expression<Func<TSource, TDest>> expr)
    {
      this.source = source;
      selectFunc = expr.Compile();
      foreach (var property in GetPropertyNames.FromExpression(expr))
      {
        DelegatePropertyChangeFrom(source, property, nameof(Value));
      }
    }

    public UIElement View() => new SelectView();
  }
}