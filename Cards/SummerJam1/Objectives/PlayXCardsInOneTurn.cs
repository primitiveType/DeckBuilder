using Api;
using CardsAndPiles;
using Newtonsoft.Json;

namespace SummerJam1.Objectives
{
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
}
