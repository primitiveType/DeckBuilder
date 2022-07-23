using System;
using App;

namespace SummerJam1
{
    public class DeckPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(GameContext.Instance.Game.Deck.Entity);
        }
    }
}
