using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Objectives
{
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
}