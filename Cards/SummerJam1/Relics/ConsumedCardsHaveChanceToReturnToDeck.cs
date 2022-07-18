using System.ComponentModel;
using System.Linq;
using Api;
using Api.Extensions;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;
using SummerJam1.Units;

namespace SummerJam1.Relics
{
    public class ConsumedCardsHaveChanceToReturnToDeck : SummerJam1Component
    {
        [OnCardExhausted]
        private void OnCardExhausted(object sender, CardExhaustedEventArgs args)
        {
            int roll = Game.Random.SystemRandom.Next(0, 10);
            if (roll > 4)
            {
                args.CardId.TrySetParent(Game.Battle.BattleDeck.Entity);
            }
        }
    }

    public class NoiseDealsDamageToEnemy : SummerJam1Component, IDescription
    {
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            Noisy noise = args.CardId.GetComponent<Noisy>();

            if (noise == null)
            {
                return;
            }

            IEntity enemy = Game.Battle.GetEnemies().FirstOrDefault();
            if (enemy != null)
            {
                enemy.GetComponent<Health>().TryDealDamage(noise.Amount, Entity);
            }
        }

        public string Description => "Noise damages enemies.";
    }

    public class DrawExtraCardsOnFirstTurn : SummerJam1Component, IDescription
    {
        public int Amount { get; set; }

        private bool IsFirstTurn { get; set; }

        [OnBattleStarted]
        private void OnBattleStarted()
        {
            IsFirstTurn = true;
        }

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            if (!IsFirstTurn)
            {
                return;
            }

            IsFirstTurn = false;

            for (int i = 0; i < Amount; i++)
            {
                Game.Battle.BattleDeck.DrawCard();
            }
        }

        public string Description => $"Draw {Amount} extra card{Amount.ToPluralitySuffix()} on your first turn each combat.";
    }

    public class RaiseMaxHp : SummerJam1Component, IDescription
    {
        public int Amount { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent))
            {
                if (Entity.Parent == Game.RelicPile.Entity)
                {
                    Health health = Game.Player.Entity.GetComponent<Health>();
                    health.SetMax(health.Max + Amount);
                }
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            Entity.PropertyChanged -= EntityOnPropertyChanged;
        }

        public string Description => $"On pickup, raise your max HP by {Amount}";
    }

    public class NoiseCardsAddPrefabToHand : SummerJam1Component, ITooltip, IDescription
    {
        public string Prefab { get; set; }
        public string PrefabPrettyName { get; set; }
        public string Tooltip => $"Whenever you play a noise card, add a {PrefabPrettyName} card to your hand.";

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId.GetComponent<Noisy>() != null)
            {
                Context.CreateEntity(Game.Battle.Hand.Entity, Prefab);
            }
        }

        public string Description => Tooltip;
    }

    public class ReachingXHealthHealsForY : SummerJam1Component, ITooltip, IDescription
    {
        public int TargetHealth { get; set; }
        public int HealAmount { get; set; }
        public string Tooltip => $"When your health reaches exactly {TargetHealth}, heal for {HealAmount}.";
        public string Description => Tooltip;

    }

    public class DealDamageEveryTurn : SummerJam1Component, ITooltip, IDescription
    {
        public int PlayerDamage { get; set; }
        public int EnemyDamage { get; set; }
        public string Description => Tooltip;

        public string Tooltip
        {
            get
            {
                if (PlayerDamage > 0)
                {
                    return $"At the end of every turn, take {PlayerDamage} damage and deal {EnemyDamage}.";
                }

                return $"At the end of every turn, deal {EnemyDamage} damage.";
            }
        }
    }

    public class DealDamageMakeNoiseAtStartOfCombat : SummerJam1Component, ITooltip, IDescription
    {
        public string Description => Tooltip;

        public int StealthAmount { get; set; }
        public int DamageAmount { get; set; }

        [OnBattleStarted]
        public void OnBattleStarted(object sender, BattleStartedEventArgs args)
        {
            Game.Player.Entity.GetComponent<Stealth>().TryUseStealth(StealthAmount);
            IEntity enemy = Game.Battle.GetEnemies().FirstOrDefault();
            enemy?.GetComponent<Health>().TryDealDamage(DamageAmount, Entity);
        }

        public string Tooltip => $"At start of battle, reduces the player's stealth by {StealthAmount} and deal {DamageAmount} damage.";
    }

    public class RaiseMaxStealth : SummerJam1Component, IDescription
    {
        public int Amount { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent))
            {
                if (Entity.Parent == Game.RelicPile.Entity)
                {
                    Game.Player.Entity.GetComponent<Stealth>().MaxStealth += Amount;
                }
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            Entity.PropertyChanged -= EntityOnPropertyChanged;
        }

        public string Description => $"On pickup, raise your max stealth by {Amount}";
    }
}
