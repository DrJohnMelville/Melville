using System;

namespace AspNetCoreLocalLog.HubLog
{
    public class NotifyEventArgs<T> : EventArgs
    {
        public NotifyEventArgs(T eventData)
        {
            EventData = eventData;
        }

        public T EventData { get; }
    }
    public interface INotifyEvent<T>
    {
        event EventHandler<NotifyEventArgs<T>>? Notify;
        void Fire(T eventData);
    }

    public class NotifyEvent<T> : INotifyEvent<T>
    {
        public event EventHandler<NotifyEventArgs<T>>? Notify;
        public void Fire(T eventData)
        {
            Notify?.Invoke(this, new NotifyEventArgs<T>(eventData));
        }
    }
}