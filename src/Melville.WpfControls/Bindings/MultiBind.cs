using  System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Melville.WpfControls.Bindings
{
  [MarkupExtensionReturnType(typeof(object))]
  public class MultiBind : MultiBinding
  {
    #region Constructors
    private void AddBinding(object item)
    {
      Bindings.Add(BindTo(item));
    }

    private static BindingBase BindTo(object item) => 
      item as BindingBase ?? new Binding { Source = item };

    public MultiBind(IMultiValueConverter converter, object b1)
    {
      Converter = converter;
      CheckOneWay();
      AddBinding(b1);
    }

    private void CheckOneWay()
    {
      var mb = Converter as MultiConverter;
      if (mb != null && !mb.HasInverseFunction)
      {
        Mode = BindingMode.OneWay;
      }
    }

    public MultiBind(IMultiValueConverter converter, object b1, object b2)
    {
      Converter = converter;
      CheckOneWay();
      AddBinding(b1);
      AddBinding(b2);
    }


    public MultiBind(IMultiValueConverter converter, object b1, object b2, object b3)
    {
      Converter = converter;
      AddBinding(b1);
      AddBinding(b2);
      AddBinding(b3);
    }

    public MultiBind(IMultiValueConverter converter, object b1, object b2, object b3,
      object b4)
    {
      Converter = converter;
      CheckOneWay();
      AddBinding(b1);
      AddBinding(b2);
      AddBinding(b3);
      AddBinding(b4);
    }

    public MultiBind(IMultiValueConverter converter, object b1, object b2, object b3,
      object b4, object b5)
    {
      Converter = converter;
      CheckOneWay();
      AddBinding(b1);
      AddBinding(b2);
      AddBinding(b3);
      AddBinding(b4);
      AddBinding(b5);
    }

    public MultiBind(IMultiValueConverter converter, object b1, object b2, object b3,
      object b4, object b5, object b6)
    {
      Converter = converter;
      CheckOneWay();
      AddBinding(b1);
      AddBinding(b2);
      AddBinding(b3);
      AddBinding(b4);
      AddBinding(b5);
      AddBinding(b6);
    }

    public MultiBind(IMultiValueConverter converter, object b1, object b2, object b3,
      object b4, object b5, object b6, object b7)
    {
      Converter = converter;
      CheckOneWay();
      AddBinding(b1);
      AddBinding(b2);
      AddBinding(b3);
      AddBinding(b4);
      AddBinding(b5);
      AddBinding(b6);
      AddBinding(b7);
    }

    public MultiBind(IMultiValueConverter converter, object b1, object b2, object b3,
      object b4, object b5, object b6, object b7, object b8)
    {
      Converter = converter;
      CheckOneWay();
      AddBinding(b1);
      AddBinding(b2);
      AddBinding(b3);
      AddBinding(b4);
      AddBinding(b5);
      AddBinding(b6);
      AddBinding(b7);
      AddBinding(b8);
    }


    public static MultiBind Create<T1, T2, TR>(Func<T1, T2, TR> func, object b1, object b2)
    {
      return new MultiBind(LambdaConverter.Create(func), b1, b2);
    }
    public static MultiBind Create<T1, T2, T3, T4, TR>(Func<T1, T2, T3, T4, TR> func, object b1, object b2, object b3, object b4)
    {
      return new MultiBind(LambdaConverter.Create(func), b1, b2, b3, b4);
    }
    #endregion
  }
}