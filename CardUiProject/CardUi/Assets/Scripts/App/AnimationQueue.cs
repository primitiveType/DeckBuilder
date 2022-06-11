using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Api;
using UnityEngine;

namespace App
{
    public class AnimationQueue : MonoBehaviourSingleton<AnimationQueue>
    {
        private Queue<Func<IEnumerator>> Queue { get; set; } = new Queue<Func<IEnumerator>>();
        private LinkedList<Func<IEnumerator>> NewQueue { get; } = new LinkedList<Func<IEnumerator>>();

        public IDisposable Enqueue(Func<IEnumerator> routine)
        {
            EventHandle<EventArgs> handle = new EventHandle<EventArgs>(null, () => { NewQueue.Remove(routine); });
            NewQueue.AddLast(routine);
            return handle;
        }

        private IEnumerator QueueRoutine(Func<IEnumerator> routine)
        {
            yield return null;
            yield return StartCoroutine((routine.Invoke()));
        }

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(AnimationLoop());
        }

        private IEnumerator AnimationLoop()
        {
            while (true)
            {
                yield return null;
                if (Queue.Any())
                {
                    Func<IEnumerator> current = Queue.Dequeue();
                    yield return current.Invoke();
                }
            }
        }
    }
}