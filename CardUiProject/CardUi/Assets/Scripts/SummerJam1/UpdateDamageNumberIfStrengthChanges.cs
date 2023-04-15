using SummerJam1;
using UnityEngine;

namespace App
{
    [RequireComponent(typeof(DisplayDamageForIntent))]
    public class UpdateDamageNumberIfStrengthChanges : ComponentView<Strength>
    {
        private DisplayDamageForIntent Display { get; set; }
        protected override void Awake()
        {
            base.Awake();
            Display = GetComponent<DisplayDamageForIntent>();
        }

        protected override void ComponentOnPropertyChanged()
        {
            Display.UpdateDisplay();
        }
    }
}