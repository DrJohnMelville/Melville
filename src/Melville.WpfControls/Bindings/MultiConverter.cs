using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Melville.MVVM.Functional;

namespace Melville.WpfControls.Bindings
{

  public partial class MultiConverter : IMultiValueConverter, IValueConverter
  {
    Delegate function;
    Delegate? inverseFunction;
    private object defaultValue = DependencyProperty.UnsetValue;

    // this class has no invariants.

    public MultiConverter HasDefault(object defaultValue)
    {
      this.defaultValue = defaultValue;
      return this;
    }
    private bool ignoreExtraArguments;
    public MultiConverter IgnoresExtraArguments()
    {
      ignoreExtraArguments = true;
      return this;
    }

    public MultiConverter(Delegate function, Delegate? inverseFunction = null)
    {
      this.function = function;
      this.inverseFunction = inverseFunction;
    }

    #region Convert and ConvertBack methods for both interfaces
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    { //IMultiValueConverter
      return DynamicInvoke(function, defaultValue, values, parameter, culture);
    }
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    { // IValueConverter
      return Convert(new[] { value }, targetType, parameter, culture);
    }
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    { // IMultiValueConverter
      if (inverseFunction == null)
        throw new InvalidOperationException("No inverse converter has been defined.");
      var result = DynamicInvoke(inverseFunction, Enumerable.Repeat(Binding.DoNothing, targetTypes.Length).ToArray(), new[] { value }, parameter, culture);
      if (!inverseFunction.Method.ReturnType.IsArray)
        result = new[] { result };
      return (object[])(result??Array.Empty<object>());
    }

    public bool HasInverseFunction => inverseFunction != null;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    { // IValueConverter
      return ConvertBack(value, new[] { targetType }, parameter, culture)[0];
    }
    #endregion

    private object? DynamicInvoke(Delegate func, object defaultResult, object[] args, object parameter, CultureInfo culture)
    {
      var adjustedValues = ComputeValidCallParameters(func, args, parameter, culture);
      try
      {
        return adjustedValues == null ? defaultResult : func.DynamicInvoke(adjustedValues);
      }
      catch (Exception)
      { // the custom in WPF is for binding failures to do so silently 
        return defaultResult;
      }
    }

    #region Parameter List creation
    private object[]? ComputeValidCallParameters(Delegate func, object[] values, object parameter, CultureInfo culture)
    {
      var paramTypes = func.Method.GetParameters().Select(i => i.ParameterType).ToArray();

      object[] adjustedValueList = ReconcileValueListLength(paramTypes, values, parameter, culture);
      return ParameterListValid(adjustedValueList, paramTypes) ? adjustedValueList : null;
    }

    private object[] ReconcileValueListLength(Type[] paramTypes, object[] values, object parameter, CultureInfo culture)
    {
      switch (paramTypes.Length - values.Length)
      { // switch on the number of additional values needed
        case 2:
          return values.Concat(parameter, culture).ToArray();
        case 1:
          return values.Concat(paramTypes[paramTypes.Length - 1] == typeof(CultureInfo) ? culture : parameter).ToArray();
        case 0: return values;
        default:
          return LengthLegalWithTrimming(values, paramTypes) ?
                                                               values.Take(paramTypes.Length).ToArray() :
                                                                                                          FunctionalMethods.Repeat(DependencyProperty.UnsetValue, paramTypes.Length).ToArray(); // param count wrong, so return a list ParameterListValid will reject
      }
    }
    private bool ParameterListValid(object[] values, Type[] paramType)
    {
      // this is a funny construct, we need the extended select form to compute the valid values, so we
      // use a vacuous condition in the all statement to accumuate all our boolean values
      return values
        .Select((value, i) => IsValidParameterForType(value, paramType[i]))
        .All(i => i);
    }
    private bool LengthLegalWithTrimming(object[] values, Type[] paramTypes)
    {
      return (values.Length > paramTypes.Length) && ignoreExtraArguments;
    }
    private static bool IsValidParameterForType(object value, Type valueType)
    {
      // we have decided, as a business rule, that any null will trigger the default value. thus
      // the user does not have to check for null.  Thus the first clause of this and is a 
      // business rule, not just a guard clause for the second clause.
      return value != null && valueType.IsAssignableFrom(value.GetType());
    }
    #endregion
  }
}