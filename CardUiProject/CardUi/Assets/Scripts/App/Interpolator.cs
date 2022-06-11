using System;
using System.Collections.Generic;
using App.Utility;
using UnityEngine;

namespace App
{
    public class Interpolator : MonoBehaviourSingleton<Interpolator>
    {
        private class InterpolationHandle : IDisposable
        {
            public InterpolatorAction Action { get; }
            public Action OnCompleteOrDispose { get; }

            public InterpolationHandle(InterpolatorAction action, Action onCompleteOrDispose)
            {
                Action = action;
                OnCompleteOrDispose = onCompleteOrDispose;
            }

            public void Dispose()
            {
                OnCompleteOrDispose?.Invoke();
            }
        }

        private void Update()
        {
            InterpolationHandle currentHandle = QueuedHandles.Peek();
            bool complete = currentHandle.Action.Invoke(Time.deltaTime);

            if (complete)
            {
                QueuedHandles.Dequeue();
            }
        }

        private Queue<InterpolationHandle> QueuedHandles = new Queue<InterpolationHandle>();

        public void QueueInterpolation(InterpolatorAction interpolateAction, Action onCompleteOrDispose,
            QueueType queueType)
        {
            if (queueType == QueueType.Instant)
            {
                foreach (InterpolationHandle handle in QueuedHandles)
                {
                    handle.Dispose();
                }

                QueuedHandles.Clear();
            }

            InterpolationHandle newHandle = new InterpolationHandle(interpolateAction, onCompleteOrDispose);
            QueuedHandles.Enqueue(newHandle);
        }
    }
}