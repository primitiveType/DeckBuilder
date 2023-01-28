using System;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1;
using SummerJam1.Cards.Effects;
using SummerJam1.Units;

namespace SummerJam1Tests
{
    public class TestUnit : Unit
    {
        protected override bool PlayCard(IEntity target)
        {
            return true;
        }

        public override bool AcceptsParent(IEntity parent)
        {
            return true;
        }
    }

    public class TestComponent : SummerJam1Component, IEffect
    {
        private int _entrancies = 0;
        protected override void Initialize()
        {
            base.Initialize();
            Logging.Logger.Log("Initializing reentrancy component.");
        }

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            Logging.Logger.Log("Reentrancy component oncardplayed.");

            if (_entrancies > 100)
            {
                return;
            }
            _entrancies++;

            args.CardId.GetComponent<Card>().TryPlayCard(args.Target);
        }

        public bool DoEffect(IEntity target)
        {
            return true;
        }
    }
}
