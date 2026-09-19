using SummerJam1;
using UnityEngine;

namespace App
{
    [RequireComponent(typeof(DisplayDamageForIntent))]
    public class UpdateDamageNumberIfStrengthChanges : ComponentView<Strength>
        //TODO: Rather than base this on strength, introduce an interface or something that can be used to indicate a change in damage calcs
    {
        private DisplayDamageForIntent Display { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Display = GetComponent<DisplayDamageForIntent>();
        }

        protected override void ComponentOnPropertyChanged()
        {
            AnimationQueue.Instance.Enqueue(() => Display.UpdateDisplay());
        }
    }
}