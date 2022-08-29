using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using External.UnityAsync.UnityAsync.Assets.UnityAsync.Await;
using SummerJam1.Units;
using UnityEngine;

namespace SummerJam1
{
    public class IntentComponentAnimator : View<Unit>
    {
        [SerializeField] private Animator m_Animator;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private List<IDisposable> Handles { get; } = new List<IDisposable>();

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
                Disposables.Add(AnimationQueue.Instance.Enqueue(Routine));
            }
        }


        private async Task Routine()
        {
            IsAttacking = true;
            m_Animator.SetTrigger(Attack);

            while (IsAttacking)
            {
                 await Await.NextUpdate();
            }
        }

        public void AttackFinished()
        {
            IsAttacking = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (IDisposable eventHandle in Handles)
            {
                eventHandle.Dispose();
            }
        }

        private void OnDisable()
        {
            m_Animator.enabled = false;
        }
    }
}
