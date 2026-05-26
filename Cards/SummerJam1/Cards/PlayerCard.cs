using System.Collections.Generic;
using System.Linq;
using Api;
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


        protected override bool PlayCard(IEntity target)
        {
            bool played = false;
            foreach (IEffect effect in Entity.GetComponents<IEffect>())
            {
                TargetingType targetingType = Entity.GetComponent<Targeting>()?.Type ?? effect.Targeting;
                foreach (IEntity resolvedTarget in ResolveTargets(targetingType, target))
                {
                    played |= effect.DoEffect(resolvedTarget);
                }
            }

            return played;
        }

        private IEnumerable<IEntity> ResolveTargets(TargetingType targetingType, IEntity requestedTarget)
        {
            switch (targetingType)
            {
                case TargetingType.None:
                    return new[] { requestedTarget ?? Entity };
                case TargetingType.Self:
                    return new[] { Entity.GetComponent<CardOwner>()?.Owner ?? Game.Player.Entity };
                case TargetingType.Ally:
                    return ResolveSingleAlly(requestedTarget);
                case TargetingType.AllAllies:
                    return Game.Party?.ActiveMembers ?? Enumerable.Empty<IEntity>();
                case TargetingType.AllEnemies:
                    return Game.Battle?.MonsterSlots.Entity.Children ?? Enumerable.Empty<IEntity>();
                case TargetingType.RandomEnemy:
                    return ResolveRandomEnemy();
                case TargetingType.Enemy:
                case TargetingType.Unit:
                default:
                    return requestedTarget == null ? Enumerable.Empty<IEntity>() : new[] { requestedTarget };
            }
        }

        private IEnumerable<IEntity> ResolveSingleAlly(IEntity requestedTarget)
        {
            if (requestedTarget == null || Game.Party == null)
            {
                return Enumerable.Empty<IEntity>();
            }

            return Game.Party.ActiveMembers.Contains(requestedTarget) ? new[] { requestedTarget } : Enumerable.Empty<IEntity>();
        }

        private IEnumerable<IEntity> ResolveRandomEnemy()
        {
            List<IEntity> enemies = Game.Battle?.MonsterSlots.Entity.Children.ToList() ?? new List<IEntity>();
            if (!enemies.Any())
            {
                return Enumerable.Empty<IEntity>();
            }

            return new[] { enemies[Game.Random.SystemRandom.Next(enemies.Count)] };
        }
    }
}
