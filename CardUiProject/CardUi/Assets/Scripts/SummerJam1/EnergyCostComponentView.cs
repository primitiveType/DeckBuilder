using System.Collections.Specialized;
using App;
using SummerJam1.Cards;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class EnergyCostComponentView : ComponentView<EnergyCost>
    {
        [SerializeField] private TMP_Text EnergyText;
        [SerializeField] private GameObject energyGo;

        protected override void Start()
        {
            base.Start();
            Entity.Components.CollectionChanged += ComponentsOnCollectionChanged;
        }

        private void ComponentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ComponentOnPropertyChanged();
           
        }

        protected override void ComponentOnPropertyChanged()
        {
            if (Component == null)
            {
                energyGo.SetActive(false);
                return;
            }
            var locked = Entity.HasComponent<CantPlayUntilEndOfTurn>();
            energyGo.SetActive(!locked);
            
            int amount = Entity.HasComponent<CardsAndPiles.Components.IFreePlayCard>() ? 0 : Component.Amount;
            Disposables.Add(AnimationQueue.Instance.Enqueue((() => SomeRoutine(amount))));
        }

        private void SomeRoutine(int cost)
        {
            EnergyText.text = cost.ToString();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Entity.Components.CollectionChanged -= ComponentsOnCollectionChanged;
        }
    }
}