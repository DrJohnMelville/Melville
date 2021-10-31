#nullable disable warnings
using Melville.MVVM.BusinessObjects;
using System;

namespace Melville.TestHelpers.Test.InpcTesting;

public sealed partial class BusinessObjectTesterTest
{
  private class MondoBusinessObject : NotifyBase
  {
    private Int16 _Int16Property;
    public Int16 Int16Property
    {
      get => _Int16Property;
      set => AssignAndNotify(ref _Int16Property, value);
    }
    private UInt16 _UInt16Property;
    public UInt16 UInt16Property
    {
      get => _UInt16Property;
      set => AssignAndNotify(ref _UInt16Property, value);
    }
    private Int32 _Int32Property;
    public Int32 Int32Property
    {
      get => _Int32Property;
      set => AssignAndNotify(ref _Int32Property, value);
    }
    private UInt32 _UInt32Property;
    public UInt32 UInt32Property
    {
      get => _UInt32Property;
      set => AssignAndNotify(ref _UInt32Property, value);
    }
    private Int64 _Int64Property;
    public Int64 Int64Property
    {
      get => _Int64Property;
      set => AssignAndNotify(ref _Int64Property, value);
    }
    private UInt64 _UInt64Property;
    public UInt64 UInt64Property
    {
      get => _UInt64Property;
      set => AssignAndNotify(ref _UInt64Property, value);
    }
    private float _floatProperty;
    public float floatProperty
    {
      get => _floatProperty;
      set => AssignAndNotify(ref _floatProperty, value);
    }
    private double _doubleProperty;
    public double doubleProperty
    {
      get => _doubleProperty;
      set => AssignAndNotify(ref _doubleProperty, value);
    }
    private decimal _decimalProperty;
    public decimal decimalProperty
    {
      get => _decimalProperty;
      set => AssignAndNotify(ref _decimalProperty, value);
    }
    private DateTime _DateTimeProperty;
    public DateTime DateTimeProperty
    {
      get => _DateTimeProperty;
      set => AssignAndNotify(ref _DateTimeProperty, value);
    }
    private bool _boolProperty;
    public bool boolProperty
    {
      get => _boolProperty;
      set => AssignAndNotify(ref _boolProperty, value);
    }
    private string _stringProperty;
    public string stringProperty
    {
      get => _stringProperty;
      set => AssignAndNotify(ref _stringProperty, value);
    }
    private TryAnEnum _TryAnEnumProperty;
    public TryAnEnum TryAnEnumProperty
    {
      get => _TryAnEnumProperty;
      set => AssignAndNotify(ref _TryAnEnumProperty, value);
    }
    private HasDefaultConstructor _HasDefaultConstructorProperty;
    public HasDefaultConstructor HasDefaultConstructorProperty
    {
      get => _HasDefaultConstructorProperty;
      set => AssignAndNotify(ref _HasDefaultConstructorProperty, value);
    }
  }
}