using System.Windows.Controls;

namespace WebDashboard.SecretManager.Views;

public interface IHasPassword
{
    public string Password();
}
public partial class RootView : UserControl, IHasPassword
{
    public RootView()
    {
        InitializeComponent();
    }
        
    public string Password() => PasswordBox.Password;
}