using  System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using Melville.TestHelpers.DeepComparisons;
using Xunit;

namespace Melville.TestHelpers.InpcTesting
{
  public static class BusinessObjectTester
  {
    /// <summary>
    /// Creates a business object Tester
    /// </summary>
    /// <typeparam name="T">Type of the object to test</typeparam>
    /// <param name="roundTripper">Round trip the object through a serializer, null if no serializer</param>
    /// <returns>A new business object tester</returns>
    public static BusinessObjectTester<T> Create<T>(Func<T, T>? roundTripper = null)
      where T : class, INotifyPropertyChanged, new() =>
      new BusinessObjectTester<T>(() => new T(), roundTripper);

    /// <summary>
    /// Creates a business object Tester
    /// </summary>
    /// <typeparam name="T">Type of the object to test</typeparam>
    /// <param name="creator">Func to create an empty object</param>
    /// <param name="roundTripper">Round trip the object through a serializer, null if no serializer</param>
    /// <returns>A new business object tester</returns>
    public static BusinessObjectTester<T> Create<T>(Func<T> creator, Func<T, T>? roundTripper = null)
      where T : class, INotifyPropertyChanged =>
      new BusinessObjectTester<T>(creator, roundTripper);
  }

  public sealed class BusinessObjectTester<TClass> where TClass : class, INotifyPropertyChanged
  {
    private readonly Func<TClass> createDefaultObject;
    private readonly List<PublicProperty> properties = new List<PublicProperty>();
    private readonly TClass defaultItem;
    private readonly Func<TClass, TClass>? roundTripper;


    /// <summary>
    /// Creates a Business Object Tester
    /// </summary>
    /// <param name="createDefaultObject">Function that creates an Undifferentiated object</param>
    /// <param name="roundTripper">Function that serializes and the deserializes the object.  If null then do not check serialization</param>
    public BusinessObjectTester(Func<TClass> createDefaultObject, Func<TClass, TClass>? roundTripper = null)
    {
      this.createDefaultObject = createDefaultObject;
      this.roundTripper = roundTripper;
      defaultItem = createDefaultObject();
    }


    /// <summary>
    /// Declare a property for testing
    /// </summary>
    /// <typeparam name="TProp">Type of the property</typeparam>
    /// <param name="inputExpr">Expression to the getter for this parameter</param>
    /// <param name="nonDefaultValue">Another, nondefault value for the object</param>
    /// <param name="otherNotifications">Other properties which should notify when this property changes</param>
    /// <returns>A BusinessObjectTester, for chaining calls</returns>
    public BusinessObjectTester<TClass> Property<TProp>(
      Expression<Func<TClass, TProp>> inputExpr, TProp nonDefaultValue,
      params Expression<Func<TClass, object>>[] otherNotifications)
    {
      properties.Add(PropertyTestRecord.Create(inputExpr, nonDefaultValue,
        defaultItem, otherNotifications));
      return this;
    }

    /// <summary>
    /// Declares that a property should be excluded from notify and serialization testing
    /// </summary>
    /// <typeparam name="TProp">Type of the property</typeparam>
    /// <param name="inputExpr">Ex0pression for the property to exclude</param>
    /// <returns>A BusinessObjectTester, for chaining calls</returns>
    public BusinessObjectTester<TClass> ExcludedProperty<TProp>(
      Expression<Func<TClass, TProp>> inputExpr)
    {
      properties.Add(new ExcludedPropertyRecord(inputExpr.Body.GetAccessedMemberName()));
      return this;
    }

    /// <summary>
    /// Run the test on all the properties of the business object
    /// </summary>
    public void DoTests()
    {
      RunAllPropertiesAccounted();
      foreach (var prop in properties)
      {
        RunSinglePropertyCheck(prop);
      }
    }

    private void RunSinglePropertyCheck(PublicProperty prop)
    {
      prop.RunINPCCheck(createDefaultObject());
      if (roundTripper != null)
      {
        prop.RunSerializationCheck(createDefaultObject, roundTripper, AssertIsSame);
      }
    }

    private bool AssertIsSame(Object o1, Object o2, bool assert) => 
      DeepComparison.AreSame(o1, o2, assert, excludedFromComparison);

    private string[] excludedFromComparison = Array.Empty<string>();

    public BusinessObjectTester<TClass> DoNotCompare(string name)
    {
      excludedFromComparison = excludedFromComparison.Append(name).ToArray();
      return this;
    }

    public BusinessObjectTester<TClass> DoNotCompare<TProp>(Expression<Func<TClass, TProp>> expr) =>
      DoNotCompare(expr.Body.GetAccessedMemberName());

    private void RunAllPropertiesAccounted()
    {
      foreach (var item in typeof(TClass).GetProperties().Where(i =>
        i.CanRead &&
        i.CanWrite && IsValidProperty(i.GetGetMethod()) && IsValidProperty(i.GetSetMethod())))
      {
        if (properties.Any(i => i.PropName.Equals(item.Name, StringComparison.Ordinal))) continue;
        if (SynthesizeValue(item.PropertyType, out var newValue))
        {
          properties.Add(new PropertyTestRecord(item, defaultItem, newValue, new string[0]));
        }
        else
        {
          Assert.True(false, item.Name + " is not tested in the property test.");
        }
      }
    }

    public static IDictionary<string, object> DefaultValues { get; } =
      new Dictionary<string, object>()
      {
        {"Int32", 7369907},
        {"UInt32", (UInt32) 7369907},
        {"Int16", (Int16) 9907},
        {"UInt16", (UInt16) 9907},
        {"Int64", (Int64) 9907},
        {"UInt64", (UInt64) 9907},
        {"Single", (float) 9907},
        {"Double", (double) 9907},
        {"Decimal", (Decimal) 9907},
        {"DateTime", new DateTime(1973, 12, 27)},
        {"Boolean", true},
        {"String", "This is an unlikely string to be the default for a business object."},
      };

    private bool SynthesizeValue(Type type, out object value)
    {
      if (DefaultValues.TryGetValue(type.Name, out value)) return true;
      if (type.IsEnum)
      {
        value = type.GetEnumValues().OfType<object>().Last();
        return true;
      }

      var constructor = type.GetConstructor(new Type[0]);
      {
        if (constructor != null)
        {
          value = constructor.Invoke(new object[0]);
          return true;
        }
      }
      return false;
    }

    private bool IsValidProperty(MethodInfo method) => method != null && method.IsPublic;
  }

  internal interface PublicProperty
  {
    string PropName { get; }
    void RunINPCCheck(INotifyPropertyChanged newItem);
    void RunSerializationCheck<T>(Func<T> creator, Func<T, T> roundTripper, 
      Func<object,object,bool,bool>  assertIsSame);
  }

  internal sealed class ExcludedPropertyRecord : PublicProperty
  {
    public ExcludedPropertyRecord(string propName)
    {
      PropName = propName;
    }

    public string PropName { get; }
    public void RunINPCCheck(INotifyPropertyChanged newItem)
    {
    }

    public void RunSerializationCheck<T>(Func<T> creator, Func<T, T> roundTripper, 
      Func<object, object, bool, bool> assertIsSame)
    {
    }
  }

  internal class PropertyTestRecord : PublicProperty 
  {
    public string PropName => setter.Name;
    private readonly string[] allNotificationProperties;
    private readonly object? defaultValue;
    private readonly object? newValue;
    private readonly PropertyInfo setter;

    public PropertyTestRecord(PropertyInfo setter, object defaultClass, object? newValue,
      string[] allNotificationProperties)
    {
      this.setter = setter;
      this.defaultValue = GetValue(defaultClass);
      this.newValue = newValue;
      this.allNotificationProperties = new[] {PropName}.Concat(allNotificationProperties).ToArray();
    }

    private object? GetValue(object target) => setter.GetValue(target);
    private void SetValue(object target, object? value) => setter.SetValue(target, value, null);

    public void RunINPCCheck(INotifyPropertyChanged newItem)
    {
      Assert.Equal(defaultValue, GetValue(newItem));
      Assert.NotEqual(defaultValue, newValue);
      using (var foo = new INPCCounter(newItem, allNotificationProperties))
      {
        SetValue(newItem, newValue);
      }

      Assert.Equal(newValue, GetValue(newItem));
    }

    public void RunSerializationCheck<T>(Func<T> creator, Func<T, T> roundTripper, Func<object, object, bool, bool> assertIsSame)
    {
      var target = creator();
      var defaultItem = creator();
      if (target == null ||defaultItem == null) throw new InvalidOperationException("Creator created a null value.");
      Assert.True(assertIsSame(target, defaultItem, true));
      SetValue(target, newValue);
      Assert.False(assertIsSame(target, defaultItem, false));

      var readItem = roundTripper(target);

      if (readItem == null) throw new InvalidOperationException("Roundtripper returned null");
      Assert.True(assertIsSame(target, readItem, true));
      Assert.False(assertIsSame(target, defaultItem, false));
    }

    public static PropertyTestRecord Create<TClass, TProp>(Expression<Func<TClass, TProp>> inputExpr,
      TProp nonDefaultValue, TClass defaultClass,
      IEnumerable<Expression<Func<TClass, object>>> otherNotifications) where TClass : INotifyPropertyChanged
    {
      return new PropertyTestRecord((PropertyInfo)inputExpr.Body.GetAccessedMemberInfo(), defaultClass,
        nonDefaultValue,
        otherNotifications.Select(i => i.GetAccessedMemberName()).ToArray());
    }
  }
}