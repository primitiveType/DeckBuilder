﻿using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Cards.Effects;

namespace SummerJam1.Cards
{
    public class PlayerCard : Card, IDraggable
    {
        protected Game Game { get; private set; }
        [JsonIgnore] public bool CanDrag => Entity?.Parent != null && Entity.Parent == Game.Battle?.Hand.Entity; //can only drag while in hand.

        protected override void Initialize()
        {
            base.Initialize();
            Game = Context.Root.GetComponent<Game>();
        }


        //TODO: will probably remove this.
        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            Health target = args.Target.GetComponentInChildren<Health>();

            if (target == null)
            {
                args.Blockers.Add(CardBlockers.INVALID_TARGET);
            }
        }

        protected override bool PlayCard(IEntity target)
        {
            bool played = false;
            foreach (IEffect effect in Entity.GetComponents<IEffect>())
            {
                played |= effect.DoEffect(target);
            }

            return played;
        }
    }
}
