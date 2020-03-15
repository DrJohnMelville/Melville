using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Melville.MVVM.BusinessObjects;
using WebDashboard.Models;

namespace WebDashboard.Views
{
    public class SecretFileEditorViewModel: NotifyBase
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
        private SemaphoreSlim semaphore = new SemaphoreSlim(1,1);
        public async Task SaveFile()
        {
            await semaphore.WaitAsync();
            {
                await using (var output = await secrets.RewriteStream())
                {
                    await JsonSerializer.SerializeAsync(output, ToJsonObject());
                }
            }
            semaphore.Release();
        }

        private Dictionary<string, string> ToJsonObject()
        {
            var dictionary = new Dictionary<string, string>();
            Root.EnviornmentDeclarations(dictionary, "");
            return dictionary;
        }

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
    }
}