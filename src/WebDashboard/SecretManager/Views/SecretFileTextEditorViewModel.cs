using System;
using System.Text.Json;
using Melville.MVVM.BusinessObjects;
using WebDashboard.SecretManager.Models;

namespace WebDashboard.SecretManager.Views;

public class SecretFileTextEditorViewModel:NotifyBase, ISecretFileEditorViewModel
{
    private readonly SecretFileHolder secrets;
    private string text = "";
    public string Text
    {
        get => text;
        set => AssignAndNotify(ref text, value, "Text", "ParseError");
    }

    public string? ParseError
    {
        get
        {
            try
            {
                secrets.ParseJsonObject(JsonDocument.Parse(Text));
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
        
    public SecretFileTextEditorViewModel(SecretFileHolder secrets)
    {
        this.secrets = secrets;
        Text = secrets.AsString();
    }

    public ISecretFileEditorViewModel CreateSwappedView() => new SecretFileEditorViewModel(secrets);
}