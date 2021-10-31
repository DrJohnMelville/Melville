using System.Windows;

namespace Melville.MVVM.Wpf;

public interface IMessageBoxWrapper
{
  MessageBoxResult Show(string message);
  MessageBoxResult Show(string message, string caption);
  MessageBoxResult Show(string message, string caption, MessageBoxButton buttons);
  MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image);
  MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image, 
    MessageBoxResult defaultResult);
  MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image, 
    MessageBoxResult defaultResult, MessageBoxOptions options);
}
public sealed class MessageBoxWrapper: IMessageBoxWrapper
{
  private readonly Window host;

  public MessageBoxWrapper(Window host)
  {
    this.host = host;
  }

  public MessageBoxResult Show(string message) => MessageBox.Show(host, message);
  public MessageBoxResult Show(string message, string caption) => MessageBox.Show(host, message, caption);

  public MessageBoxResult Show(string message, string caption, MessageBoxButton buttons) =>
    MessageBox.Show(host, message, caption, buttons);

  public MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image) =>
    MessageBox.Show(message, caption, buttons, image);

  public MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image,
    MessageBoxResult defaultResult) =>
    MessageBox.Show(host, message, caption, buttons, image, defaultResult);

  public MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image,
    MessageBoxResult defaultResult, MessageBoxOptions options) =>
    MessageBox.Show(host, message, caption, buttons, image, defaultResult, options);
}