using CardsAndPiles.Components;

namespace App
{
    public class CardView : PileItemView<Card>, ISetModel
    {
        // public override bool TrySendToPile(IPileView pileView)
        // {
        //     return Model.TryPlayCard(pileView.Model.Entity);
        // }

        protected override void Start()
        {
            base.Start();
            //name = $"{Entity.GetComponent<Card>().GetType().Name} ({Entity.Parent.GetComponent<Pile>()?.GetType().Name})";
        }
    }
}
