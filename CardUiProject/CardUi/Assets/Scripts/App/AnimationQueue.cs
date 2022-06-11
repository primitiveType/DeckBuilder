using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using App.Utility;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using External.UnityAsync.UnityAsync.Assets.UnityAsync.Await;
using External.UnityAsync.UnityAsync.Assets.UnityAsync.AwaitInstructions;
using UnityEngine;

namespace App
{
    public class AnimationQueue : MonoBehaviourSingleton<AnimationQueue>
    {
        private LinkedList<Func<Task>> Queue { get; } = new LinkedList<Func<Task>>();
        private bool Running => true;

        public IDisposable Enqueue(Action action)
        {
            return Enqueue(() => { action(); return Task.CompletedTask; });
        }
        public IDisposable Enqueue(Func<Task> routine)
        {
            EventHandle<EventArgs> handle = new EventHandle<EventArgs>(null, () => { Queue.Remove(routine); });
            Queue.AddLast(routine);
            return handle;
        }


        protected override void Awake()
        {
            base.Awake();
            AnimationLoop();
        }

        private async void AnimationLoop()
        {
            while (Running)
            {
                try
                {
                    while (Queue.Any())
                    {
                        Func<Task> current = Queue.First();
                        Queue.RemoveFirst();
                        await current();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
                await Await.NextUpdate();
            }
        }
    }
}