using System.Threading.Tasks;
using Api;
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

        protected override void ComponentOnPropertyChanged()
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            ValueChanged(Component.GetEffectiveDamage(GameContext.Instance.Game.Player.Entity));
        }
    }
}
