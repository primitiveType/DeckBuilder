using App;
using SummerJam1.Cards;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class EnergyCostComponentView : ComponentView<EnergyCost>
    {
        [SerializeField] private TMP_Text EnergyText;

        protected override void ComponentOnPropertyChanged()
        {
            if (Component == null)
            {
                EnergyText.gameObject.SetActive(false);
                return;
            }
            int amount = Component.Amount;
            Disposables.Add(AnimationQueue.Instance.Enqueue(( ()=>SomeRoutine(amount))));
        }

        private void SomeRoutine(int cost)
        {
            EnergyText.text = cost.ToString();
        }
    }
}
