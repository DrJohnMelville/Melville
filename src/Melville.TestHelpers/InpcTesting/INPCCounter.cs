using  System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Melville.TestHelpers.InpcTesting;

public sealed class INPCCounter : IDisposable
{
    private INotifyPropertyChanged target;
    private string[] propertySequence;
    private List<string> calls = new List<string>();
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
        calls.Add(e.PropertyName);
        if (calls.Count >= propertySequence.Length)
        {
            notificationDone.SetResult(0);
        }
    }

    #region Implementation of IDisposable
    public void Dispose()
    {
        Assert.Equal(FormatCalls(propertySequence), FormatCalls(calls));
            
        target.PropertyChanged -= InpcCalled;
    }

    private string FormatCalls(IEnumerable<string> elts) => string.Join(", ", elts);

    #endregion

    public static INPCCounter VerifyInpcFired<TTarget>(TTarget target,
        params Expression<Func<TTarget, object>>[] otherProperties) 
    {
        return new INPCCounter((INotifyPropertyChanged)target,
            otherProperties.Select(accessor => accessor.Body.GetAccessedMemberName()).ToArray());
    }
}