using  System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Melville.TestHelpers.InpcTesting
{
    public sealed class INPCCounter : IDisposable
    {
        private INotifyPropertyChanged target;
        private string[] propertySequence;
        private int propChangedCount;
        private TaskCompletionSource<int> notificationDone;
        public Task NotificationsDone { get { return notificationDone.Task; } }

        public INPCCounter(INotifyPropertyChanged target, string[] propertySequence)
        {
            this.target = target;
            this.propertySequence = propertySequence;
            notificationDone = new TaskCompletionSource<int>();

            this.target.PropertyChanged += InpcCalled;
        }

        private void InpcCalled(object sender, PropertyChangedEventArgs e)
        {
            Assert.True(propChangedCount < propertySequence.Length, "Unexpected Property: " + e.PropertyName);
            Assert.Equal(propertySequence[propChangedCount], e.PropertyName); // assert the right name
            propChangedCount++; // count the invocations
            if (propChangedCount >= propertySequence.Length)
            {
                notificationDone.SetResult(0);
            }
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
            Assert.Equal(propertySequence.Length, propChangedCount);
            target.PropertyChanged -= InpcCalled;
        }
        #endregion

        public static INPCCounter VerifyInpcFired<TTarget>(TTarget target,
          params Expression<Func<TTarget, object>>[] otherProperties) 
        {
            return new INPCCounter((INotifyPropertyChanged)target,
              otherProperties.Select(accessor => accessor.Body.GetAccessedMemberName()).ToArray());
        }
    }
}
