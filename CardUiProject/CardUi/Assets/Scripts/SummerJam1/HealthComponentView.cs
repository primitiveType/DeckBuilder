﻿using App;
using Simple_Health_Bar.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace SummerJam1
{
    public class HealthComponentView : ComponentView<Health>
    {
        [SerializeField] private SimpleHealthBar HealthText;

        protected override void ComponentOnPropertyChanged()
        {
            int amount = Component.Amount;
            int max = Component.Max;
            Disposables.Add(AnimationQueue.Instance.Enqueue(() => SomeRoutine(amount, max)));
        }

        private void SomeRoutine(int health, int max)
        {
            HealthText.UpdateBar(health, max);
        }
    }
}
