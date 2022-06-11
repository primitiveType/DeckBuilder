using System.Collections.Generic;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class SummerJam1CardView : PileItemView<SummerJam1Card>, ISetModel
    {
        public override bool TrySendToPile(IPileView pileView)
        {
            return Model.TryPlayCard(pileView.Model.Entity);
        }
        
    }
}