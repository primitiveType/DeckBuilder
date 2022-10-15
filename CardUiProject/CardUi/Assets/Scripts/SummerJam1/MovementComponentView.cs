using App;
using Simple_Health_Bar.Scripts;
using UnityEngine;

namespace SummerJam1
{
    public class MovementComponentView : ComponentView<Player>
    {
        [SerializeField] private SimpleHealthBar MovementText;

        protected override void ComponentOnPropertyChanged()
        {
            if (Component == null)
            {
                gameObject.SetActive(false);
                return;
            }

            int amount = Component.Movements;
            Disposables.Add(AnimationQueue.Instance.Enqueue(() => SomeRoutine(amount, Player.MovementsPerTurn)));
        }

        private void SomeRoutine(int health, int max)
        {
            MovementText.UpdateBar(health, max);
        }
    }
}