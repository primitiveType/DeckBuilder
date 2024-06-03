using System;

namespace Api
{
    public class EventHandle<T> : IDisposable
    {
        public EventHandle(EventHandleDelegate<T> action, Action onDispose)
        {
            Action = action;
            OnDispose = onDispose;
        }

        private EventHandleDelegate<T> Action { get; }
        private Action OnDispose { get; set; }

        private bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                OnDispose.Invoke();
                OnDispose = null;
            }
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
                    Logging.LogError($"Caught exception invoking event handle! {e}");
                }
            }
        }
    }
}
