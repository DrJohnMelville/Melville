using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.FileSystem;
using Melville.MVVM.Functional;

namespace WebDashboard.Models
{
    public sealed class SecretFileHolder
    {
        private readonly IFile file;
        public SecretNode Root { get; } = new SecretNode("");

        public SecretFileHolder(IFile file, JsonDocument data)
        {
            this.file = file;
            ParseJsonObject(data);
        }

        private void ParseJsonObject(JsonDocument data)
        {
            foreach (var property in data.RootElement.EnumerateObject())
            {
                Root.AddChild(property.Name, new SecretValue("", property.Value.GetString()));
            }
            
            Root.SortThisAndChildren();
        }

        public void AddSecrets(IDictionary<string, string> target) => Root.EnviornmentDeclarations(target, "");

        public Task<Stream> RewriteStream() => file.CreateWrite();
    }

    public interface ISecretFileValue
    {
        string Name { get; set; }
        string Value { get; set; }
        IList<ISecretFileValue> Children { get; }
        void EnviornmentDeclarations(IDictionary<string, string> declarations, string prefix);
    }
    
    public sealed class SecretValue:ISecretFileValue
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public SecretValue(string namr, string value)
        {
            Value = value;
            Name = namr;
        }

        public IList<ISecretFileValue> Children => Array.Empty<ISecretFileValue>();

        public void EnviornmentDeclarations(IDictionary<string, string> declarations, string prefix) => 
            declarations[prefix+Name]= Value;
    }
    public sealed class SecretNode:ISecretFileValue{
        public string Name { get; set; }

        public string Value
        {
            get => "";
            set {}
        }

        public IList<ISecretFileValue> Children { get; } = new ThreadSafeBindableCollection<ISecretFileValue>();

        public SecretNode(string name)
        {
            Name = name;
        }
        
        public void SortThisAndChildren()
        {
            var sorted = Children.OrderBy(i => i.Name).ToList();
            Children.Clear();
            Children.AddRange(sorted);
            foreach (var node in Children.OfType<SecretNode>())
            {
                node.SortThisAndChildren();
            }
        }

        public void AddChild(ReadOnlySpan<char> name, ISecretFileValue child)
        {
            var firstColon = name.IndexOf(':');
            if (firstColon < 0)
            {
                child.Name = name.ToString();
                Children.Add(child);
            }
            else
            {
                AddToSubChild(name, child, firstColon);
            }
        }

        private void AddToSubChild(ReadOnlySpan<char> name, ISecretFileValue child, int firstColon) =>
            GetOrCreateChildList(FirstNameFrom(name, firstColon))
                .AddChild(RemainingNamesFrom(name, firstColon), child);

        private static string FirstNameFrom(ReadOnlySpan<char> name, int firstColon) => 
            name[..firstColon].ToString();

        private static ReadOnlySpan<char> RemainingNamesFrom(ReadOnlySpan<char> name, int firstColon) => 
            name[(firstColon+1)..];

        private SecretNode GetOrCreateChildList(string name)
        {
            if (GetExistingNode(name, out var ret)) 
                return (ret as SecretNode)?? throw new InvalidDataException("A name and a node cannot share a value");
            var newNode = new SecretNode(name);
            Children.Add(newNode);
            return newNode;
        }

        private bool GetExistingNode(string name, [NotNullWhen(true)] out ISecretFileValue? output)
        {
            output = null!;
            if (Children.FirstOrDefault(i => i.Name.Equals(name)) is {} foundItem)
            {
                output = foundItem;
                return true;
            }

            return false;
        }

        public void EnviornmentDeclarations(IDictionary<string, string> declarations, string prefix)
        {
            var newPrefix = Name.Length == 0? prefix : $"{prefix}{Name}:";
            foreach (var child in Children)
            {
                child.EnviornmentDeclarations(declarations, newPrefix);
            }
        }
    }
}