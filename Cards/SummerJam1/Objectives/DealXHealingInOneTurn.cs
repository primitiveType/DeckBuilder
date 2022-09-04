using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Objectives
{
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
                Game.Player.Entity == (args.SourceEntityId))
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
