using System.Threading.Tasks;
using Api;
using App.Utility;
using SummerJam1;
using UnityEditor.EditorTools;
using UnityEditor.PackageManager;

namespace App
{
    public class DisplayDamageForIntent : AmountComponentView<DamageIntent>
    {
        protected override void Awake()
        {
            base.Awake();
            gameObject.AddComponent<UpdateDamageNumberIfStrengthChanges>();
        }

        protected override void Start()
        {
            var model = GetComponentInParent<IntentComponentView>().Model;
            Component = model as DamageIntent;
            base.Start();
        }

        protected override void ComponentOnPropertyChanged()
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            ValueChanged(Component?.GetEffectiveDamage(GameContext.Instance.Game.Player.Entity));
        }

        protected override string GetStringForAmount(int? amount)
        {
            if (amount == null)
            {
                return null;
            }

            return $"{Emojis.Attack}{amount}";
        }
    }
}