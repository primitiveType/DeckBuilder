using System;

namespace Api
{
    public class EventHandle : IDisposable
    {
        private Action OnDispose { get; set; }

        public EventHandle(Action onDispose)
        {
            OnDispose = onDispose;
        }

        public void Dispose()
        {
            OnDispose.Invoke();
            OnDispose = null;
        }
    }
}