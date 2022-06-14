using App;
using CardsAndPiles;
using UnityEngine;

namespace SummerJam1
{
    public class PrizePileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(SummerJam1Context.Instance.Game.PrizePile.Entity);
        }
    }
}
