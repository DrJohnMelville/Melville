
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Melville.MVVM.Wpf.Bindings {
  public delegate R Func<T1, T2, T3, T4, T5, R>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5);
  public static class LambdaConverter {
    public static MultiConverter Create< T1, R >(Func< T1, R> func, Func<R, T1> inverse) {
      return new MultiConverter(func, inverse);
    }
    public static MultiConverter Create< T1, R >(Func< T1, R > func) {
      return new MultiConverter(func);
    }
    public static MultiConverter Create< T1, R >(Func< T1, R > func, Func<R, object[]> inverse) {
      return new MultiConverter(func, inverse);
    }
      public static MultiConverter Create< T1, T2, R >(Func< T1, T2, R > func) {
      return new MultiConverter(func);
    }
    public static MultiConverter Create< T1, T2, R >(Func< T1, T2, R > func, Func<R, object[]> inverse) {
      return new MultiConverter(func, inverse);
    }
      public static MultiConverter Create< T1, T2, T3, R >(Func< T1, T2, T3, R > func) {
      return new MultiConverter(func);
    }
    public static MultiConverter Create< T1, T2, T3, R >(Func< T1, T2, T3, R > func, Func<R, object[]> inverse) {
      return new MultiConverter(func, inverse);
    }
      public static MultiConverter Create< T1, T2, T3, T4, R >(Func< T1, T2, T3, T4, R > func) {
      return new MultiConverter(func);
    }
    public static MultiConverter Create< T1, T2, T3, T4, R >(Func< T1, T2, T3, T4, R > func, Func<R, object[]> inverse) {
      return new MultiConverter(func, inverse);
    }
      public static MultiConverter Create< T1, T2, T3, T4, T5, R >(Func< T1, T2, T3, T4, T5, R > func) {
      return new MultiConverter(func);
    }
    public static MultiConverter Create< T1, T2, T3, T4, T5, R >(Func< T1, T2, T3, T4, T5, R > func, Func<R, object[]> inverse) {
      return new MultiConverter(func, inverse);
    }
    }
  
  public partial class MultiConverter {
      public MultiConverter Inverse<T1, R> (Func< T1, R > func) {
      inverseFunction = func;
      return this;
    }  
      public MultiConverter Inverse<T1, T2, R> (Func< T1, T2, R > func) {
      inverseFunction = func;
      return this;
    }  
      public MultiConverter Inverse<T1, T2, T3, R> (Func< T1, T2, T3, R > func) {
      inverseFunction = func;
      return this;
    }  
      public MultiConverter Inverse<T1, T2, T3, T4, R> (Func< T1, T2, T3, T4, R > func) {
      inverseFunction = func;
      return this;
    }  
      public MultiConverter Inverse<T1, T2, T3, T4, T5, R> (Func< T1, T2, T3, T4, T5, R > func) {
      inverseFunction = func;
      return this;
    }  
    }
}