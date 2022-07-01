using App;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class SummerJam1CardView : PileItemView<SummerJam1Card>, ISetModel
    {
        public override bool TrySendToPile(IPileView pileView)
        {
            return Model.TryPlayCard(pileView.Model.Entity);
        }

        protected override void Start()
        {
            base.Start();
            name = $"{Entity.GetComponent<Card>().GetType().Name} ({Entity.Parent.GetComponent<Pile>().GetType().Name})";
        }
    }
    
}
