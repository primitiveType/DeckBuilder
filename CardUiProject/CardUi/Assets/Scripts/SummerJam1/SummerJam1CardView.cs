using System;
using Api;
using App;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;
using SummerJam1.Relics;

namespace SummerJam1
{
    public class SummerJam1CardView : PileItemView<Card>, ISetModel
    {
        public override bool TrySendToPile(IEntity target)
        {
            if (target != null && target.HasComponent<BagOfHoldingHandPile>())//TODO: how do you even generalize this.
            {
                //I should rework all this so that cards don't have to target "Piles". It should play the card
                //when targeting an entity directly, and try and move it when targeting a pile.
                return Model.Entity.TrySetParent(target);
            }
            return Model.TryPlayCard(target);
        }

        protected override void Start()
        {
            base.Start();
            try
            {
                name = $"{Entity.GetComponent<Card>().GetType().Name} {Entity.GetComponent<NameComponent>().Value} ({Entity.Parent.GetComponent<Pile>().GetType().Name})";
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
    
}
