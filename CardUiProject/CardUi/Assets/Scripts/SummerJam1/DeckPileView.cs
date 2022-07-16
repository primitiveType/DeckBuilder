using System;
using App;

namespace SummerJam1
{
    public class DeckPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(SummerJam1Context.Instance.Game.Deck.Entity);
        }
    }
}
