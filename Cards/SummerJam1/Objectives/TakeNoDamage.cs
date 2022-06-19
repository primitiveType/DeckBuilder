using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Objectives
{
    public abstract class Objective : SummerJam1Component
    {
        [JsonProperty] public bool Completed { get; set; }
        [JsonProperty] public bool Failed { get; set; }
    }

    public class TakeNoDamage : Objective
    {
        [OnBattleStarted]
        private void OnBattleStart()
        {
            Completed = true;
        }

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.EntityId == Game.Player.Entity)
            {
                Failed = true;
            }
        }
    }

    public class DealXDamageInOneTurn : Objective, IAmount
    {
        [JsonProperty] public int Amount { get; set; } = 0;
        [JsonProperty] public int Required { get; set; } = 15;

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Amount = 0;
        }

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Game.Player.Entity || args.SourceEntityId.GetComponent<Card>() != null ||
                Game.Battle.GetFriendlies().Contains(args.SourceEntityId))
            {
                Amount += args.Amount;
            }


            if (Amount >= Required)
            {
                Completed = true;
            }
        }
    }

    public class PlayXCardsInOneTurn : Objective, IAmount
    {
        [JsonProperty] public int Amount { get; set; } = 0;
        [JsonProperty] public int Required { get; set; } = 4;

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Amount = 0;
        }

        [OnCardPlayed]
        private void OnCardPlayed()
        {
            Amount++;
            if (Amount >= Required)
            {
                Completed = true;
            }
        }
    }

    public class DealXHealingInOneTurn : Objective, IAmount
    {
        [JsonProperty] public int Amount { get; set; } = 0;
        [JsonProperty] public int Required { get; set; } = 15;

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Amount = 0;
        }

        [OnHealDealt]
        private void OnHealDealt(object sender, HealDealtEventArgs args)
        {
            if (args.SourceEntityId == Game.Player.Entity || args.SourceEntityId.GetComponent<Card>() != null ||
                Game.Battle.GetFriendlies().Contains(args.SourceEntityId))
            {
                Amount += args.Amount;
            }


            if (Amount >= Required)
            {
                Completed = true;
            }
        }
    }

    // public class HaveAUnitWithXHealth : Objective
    // {
    // }

    public class EndWithNoUnits : Objective
    {
        [OnBattleEnded]
        private void OnBattleEnded()
        {
            if (Game.Battle.GetFriendlies().Count == 0)
            {
                Completed = true;
            }
        }
    }

    public class EndWithThreeUnits : Objective
    {
        [OnBattleEnded]
        private void OnBattleEnded()
        {
            if (Game.Battle.GetFriendlies().Count > 2)
            {
                Completed = true;
            }
        }
    }
}
