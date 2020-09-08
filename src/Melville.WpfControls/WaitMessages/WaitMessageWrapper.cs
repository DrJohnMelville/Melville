using  System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Melville.MVVM.Wpf.Bindings;

namespace Melville.WpfControls.WaitMessages
{
  public interface IWaitingService 
  {
    string? WaitMessage { get; set; }
    string? ErrorMessage { get; set; }
    int Total { get; set; }
    int Progress { get; set; }
    CancellationToken CancellationToken { get; }
    IDisposable WaitBlock(string message);
    void MakeProgress();
  }

  [Obsolete("There is a better version of this is Melville.MVVM.WPF")]
  public class WaitMessageWrapper : ContentControl, IWaitingService
  {
    static WaitMessageWrapper()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitMessageWrapper), new FrameworkPropertyMetadata(typeof(WaitMessageWrapper)));
    }


    public string? WaitMessage
    {
      get { return (string)GetValue(WaitMessageProperty); }
      set { SetValue(WaitMessageProperty, value); }
    }
    public static readonly DependencyProperty WaitMessageProperty =
        DependencyProperty.Register("WaitMessage", typeof(string), typeof(WaitMessageWrapper), new PropertyMetadata(null));



    public string? ErrorMessage
    {
      get { return (string)GetValue(ErrorMessageProperty); }
      set { SetValue(ErrorMessageProperty, value); }
    }
    public static readonly DependencyProperty ErrorMessageProperty =
        DependencyProperty.Register("ErrorMessage", typeof(string), typeof(WaitMessageWrapper), new PropertyMetadata(null));
    
    public IDisposable WaitBlock(string message)
    {
      WaitMessage = message;
      ErrorMessage = null;
      return new WaitingDisposable(this);
    }


    #region Progress


    public int Total
    {
      get { return (int)GetValue(TotalProperty); }
      set { SetValue(TotalProperty, value); }
    }
    public static readonly DependencyProperty TotalProperty =
        DependencyProperty.Register("Total", typeof(int), typeof(WaitMessageWrapper), new PropertyMetadata(int.MinValue));

      //    public bool ShowProgress => Total > int.MinValue;
    public readonly static IValueConverter ShowProgress = LambdaConverter.Create((int i) => i > int.MinValue);
 

    public int Progress
    {
      get { return (int)GetValue(ProgressProperty); }
      set { SetValue(ProgressProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Progress.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProgressProperty =
        DependencyProperty.Register("Progress", typeof(int), typeof(WaitMessageWrapper), new PropertyMetadata(0));

    public void MakeProgress() => Progress++;

    public CancellationToken CancellationToken => CancellationToken.None;
    #endregion


    private sealed class WaitingDisposable : IDisposable
    {
      private readonly IWaitingService screen;

      public WaitingDisposable(IWaitingService screen)
      {
        this.screen = screen;
      }

      public void Dispose()
      {
        screen.WaitMessage = null;
      }
    }

  }
}
