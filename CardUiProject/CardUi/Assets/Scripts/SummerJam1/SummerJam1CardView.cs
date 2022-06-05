namespace SummerJam1
{
    public class SummerJam1CardView : PileItemView<SummerJam1Card>
    {
        public override bool TrySendToPile(IPileView pileView)
        {
            return Model.TryPlayCard(pileView.Model.Entity);
        }
        
    }
}