using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App
{
    public class AnimationQueue : MonoBehaviourSingleton<AnimationQueue>
    {
        private Queue<Func<IEnumerator>> Queue { get; set; } = new Queue<Func<IEnumerator>>();
        public void Enqueue(Func<IEnumerator> routine)
        {
            Queue.Enqueue(routine);
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
                if(Queue.Any())
                {
                    var current = Queue.Dequeue();
                    yield return current.Invoke();
                }
            }
        }
    }
}