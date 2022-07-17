using System.ComponentModel;
using System.Linq;
using Api;
using Api.Extensions;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

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
                    Game.Player.MaxStealth += Amount;
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
