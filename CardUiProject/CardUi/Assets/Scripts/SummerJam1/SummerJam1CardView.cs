using System;
using Api;
using App;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class SummerJam1CardView : PileItemView<Card>, ISetModel
    {
        public override bool TrySendToPile(IEntity pileView)
        {
            return Model.TryPlayCard(pileView);
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
