using System;
using System.Linq;
using System.Threading.Tasks;
using Melville.MVVM.BusinessObjects;
using WebDashboard.SecretManager.Models;

namespace WebDashboard.SecretManager.Views;

public interface ISecretFileEditorViewModel
{
    ISecretFileEditorViewModel CreateSwappedView();
}
public class SecretFileEditorViewModel: NotifyBase, ISecretFileEditorViewModel
{
    private readonly SecretFileHolder secrets;
    public SecretNode Root => secrets.Root;

    private ISecretFileValue? current;
    public ISecretFileValue? Current
    {
        get => current;
        set => AssignAndNotify(ref current, value);
    }

    public SecretFileEditorViewModel(SecretFileHolder secrets)
    {
        this.secrets = secrets;
    }

    // clicking more than once corrupts the file, so serialize the writes.
    public Task SaveFile() => secrets.SaveFile();
    public void NewNode() => InsertNewItem(new SecretNode("Name"));
    public void NewValue() => InsertNewItem(new SecretValue("ItemName","Value"));
    public void DeleteCurrentNode()
    {
        if (Current == null) return;
        SearchForParent(Current).Children.Remove(Current);
        Current = null;
    }

    private void InsertNewItem(ISecretFileValue item)
    { 
        NewNodeParent().Children.Add(item);
        Current = item;
    }

    private SecretNode NewNodeParent() =>
        Current switch
        {
            SecretNode node => node,
            ISecretFileValue v => SearchForParent(v),
            _ => Root
        };

    private SecretNode SearchForParent(ISecretFileValue item) => SearchForParent(item, Root) ??
                                                                 throw new InvalidOperationException("No parent found for item");

    private SecretNode? SearchForParent(ISecretFileValue item, SecretNode node)
    {
        if (node.Children.Contains(item)) return node;
        return node.Children
            .OfType<SecretNode>()    
            .Select(child => SearchForParent(item, child))
            .FirstOrDefault(i => i != null);
    }

    public ISecretFileEditorViewModel CreateSwappedView()
    {
        return new SecretFileTextEditorViewModel(secrets);
    }
}