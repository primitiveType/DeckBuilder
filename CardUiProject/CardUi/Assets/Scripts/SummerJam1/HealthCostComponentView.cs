using App;
using SummerJam1.Cards;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class HealthCostComponentView : ComponentView<HealthCost>
    {
        [SerializeField] private TMP_Text HealthText;

        protected override void ComponentOnPropertyChanged()
        {
            if (Component == null)
            {
                HealthText.gameObject.SetActive(false);
                return;
            }
            int amount = Component.Amount;
            Disposables.Add(AnimationQueue.Instance.Enqueue(( ()=>SomeRoutine(amount))));
        }

        private void SomeRoutine(int cost)
        {
            HealthText.text = cost.ToString();
        }
    }
}
