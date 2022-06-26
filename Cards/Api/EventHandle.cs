using System;

namespace Api
{
    public delegate void EventHandleDelegate<T>(object sender, T item);

    public class EventHandle<T> : IDisposable 
    {
        private EventHandleDelegate<T> Action { get; }
        private Action OnDispose { get; set; }
        
        private bool IsDisposed { get; set; }


        public EventHandle(EventHandleDelegate<T> action, Action onDispose)
        {
            Action = action;
            OnDispose = onDispose;
        }

        public void Invoke(object sender, T args)
        {
            if (!IsDisposed)
            {
                try
                {
                    Action.Invoke(sender, args);
                }
                catch (Exception e)
                {
                    Logging.LogError($"Caught exception invoking event handle! {e.Message}: {e.StackTrace}");
                }
            }
        }
        
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                OnDispose.Invoke();
                OnDispose = null;
            }
        }
    }
}
