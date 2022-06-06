using System.Collections;
using CardsAndPiles.Components;
using UnityEngine;

namespace App
{
    public class HealthComponentView : ComponentView<Health>
    {
        [SerializeField] private SimpleHealthBar HealthText;

        protected override void ComponentOnPropertyChanged()
        {
            int amount = Component.Amount;
            int max = Component.Max;
            AnimationQueue.Instance.Enqueue(() => SomeRoutine(amount, max));
        }

        private IEnumerator SomeRoutine(int health, int max)
        {
            HealthText.UpdateBar(health, max);
            yield return null;
        }
    }
    
 
}