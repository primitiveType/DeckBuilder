using App;
using Simple_Health_Bar.Scripts;
using UnityEngine;

namespace SummerJam1
{
    public class StealthComponentView : ComponentView<Stealth>
    {
        [SerializeField] private SimpleHealthBar StealthText;

        protected override void ComponentOnPropertyChanged()
        {
            if (Component == null)
            {
                gameObject.SetActive(false);
                return;
            }

            int amount = Component.CurrentStealth;
            int max = Component.MaxStealth;
            Disposables.Add(AnimationQueue.Instance.Enqueue(() => SomeRoutine(amount, max)));
        }

        private void SomeRoutine(int health, int max)
        {
            StealthText.UpdateBar(health, max);
        }
    }
}
