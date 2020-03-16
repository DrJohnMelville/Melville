using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection
{
    public class ServiceCollection : IServiceCollection
    {
        private List<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();
        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return descriptors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) descriptors).GetEnumerator();
        }

        public void Add(ServiceDescriptor item)
        {
            descriptors.Add(item);
        }

        public void Clear()
        {
            descriptors.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return descriptors.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            descriptors.CopyTo(array, arrayIndex);
        }

        public bool Remove(ServiceDescriptor item)
        {
            return descriptors.Remove(item);
        }

        public int Count => descriptors.Count;

        public bool IsReadOnly => ((ICollection<ServiceDescriptor>) descriptors).IsReadOnly;

        public int IndexOf(ServiceDescriptor item) => descriptors.IndexOf(item);

        public void Insert(int index, ServiceDescriptor item) => descriptors.Insert(index, item);

        public void RemoveAt(int index) => descriptors.RemoveAt(index);

        public ServiceDescriptor this[int index]
        {
            get => descriptors[index];
            set => descriptors[index] = value;
        }
    }
}