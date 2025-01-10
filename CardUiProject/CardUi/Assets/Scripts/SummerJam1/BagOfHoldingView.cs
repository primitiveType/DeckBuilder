using App;
using CardsAndPiles;
using SummerJam1;
using SummerJam1.Relics;

public class BagOfHoldingView : PileView
{
    protected override void Start()
    {
        var bag = GameContext.Instance.Game.Player.Entity.GetComponent<BagOfHolding>();
        if (bag != null)
        {
            SetModel(bag.Pile);
        }
    }
}
