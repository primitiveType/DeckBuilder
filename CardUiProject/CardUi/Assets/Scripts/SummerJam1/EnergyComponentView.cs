using App;
using Simple_Health_Bar.Scripts;
using UnityEngine;

namespace SummerJam1
{
    public class EnergyComponentView : ComponentView<Player>
    {
        [SerializeField] private SimpleHealthBar EnergyText;

        protected override void ComponentOnPropertyChanged()
        {
            if (Component == null)
            {
                gameObject.SetActive(false);
                return;
            }

            int amount = Component.CurrentEnergy;
            int max = Component.MaxEnergy;
            Disposables.Add(AnimationQueue.Instance.Enqueue(() => SomeRoutine(amount, max)));
        }

        private void SomeRoutine(int health, int max)
        {
            EnergyText.UpdateBar(health, max);
        }
    }
}