using System.Collections;
using System.Collections.Generic;
using Api;
using Common;
using SummerJam1;
using SummerJam1.Units;
using UnityEngine;

namespace App
{
    public class IntentComponentAnimator : View<Unit>
    {
        [SerializeField] private Animator m_Animator;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private List<EventHandle> Handles { get; } = new List<EventHandle>();
        
        private bool IsAttacking { get; set; }

        protected override void Start()
        {
            base.Start();
            Handles.Add(((SummerJam1Events)Entity.Context.Events).SubscribeToIntentStarted(IntentStarted));
        }

        private void IntentStarted(object sender, IntentStartedEventArgs args)
        {
            if (args.Entity == Entity)
            {
                AnimationQueue.Instance.Enqueue(Routine);
            }
        }

        private IEnumerator Routine()
        {
            IsAttacking = true;
            m_Animator.SetTrigger(Attack);

            while (IsAttacking)
            {
                yield return null;
            }
        }

        public void AttackFinished()
        {
            IsAttacking = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (EventHandle eventHandle in Handles)
            {
                eventHandle.Dispose();
            }
        }
    }
}