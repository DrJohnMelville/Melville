using  System;
using System.ComponentModel;

namespace Melville.MVVM.BusinessObjects
{
  public static class NotifyPropertyChangedOperations
  {
    public static Action WhenMemberChanges(this INotifyPropertyChanged target, string member, Action action)
    {
      VerifyPropertyExists.InDebugBuilds((object) target, member);
      PropertyChangedEventHandler method = (s, e) =>
      {
        if ( e.PropertyName == null || e.PropertyName.Equals(member, StringComparison.Ordinal))
        {
          action();
        }
      };
      target.PropertyChanged += method;

      return () => target.PropertyChanged -= method;
    }
    public static void WhenMemberChangesOnce(this INotifyPropertyChanged target, string member, Action action)
    {
      VerifyPropertyExists.InDebugBuilds((object) target, member);
      void method(object? s, PropertyChangedEventArgs e)
      {
        if ( e.PropertyName == null || e.PropertyName.Equals(member, StringComparison.Ordinal))
        {
          action();
          target.PropertyChanged -= method;
        }
      };
      target.PropertyChanged += method;
    }

  }
}