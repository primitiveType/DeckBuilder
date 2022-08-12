using App;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class SummerJam1CardView : PileItemView<Card>, ISetModel
    {
        // public override bool TrySendToPile(IPileView pileView)
        // {
        //     if (pileView == null || !Model.TryPlayCard(pileView.Model.Entity))
        //     {
        //         return Model.TryPlayCard(GameContext.Instance.Game.Player.Entity);//assume the card is meant for the player.
        //     }
        //
        //     return false;
        // }
    
        protected override void Start()
        {
            base.Start();
            name = $"{Entity.GetComponent<Card>().GetType().Name} ({Entity.Parent.GetComponent<Pile>().GetType().Name})";
        }
    }
    
}
