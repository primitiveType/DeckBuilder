using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Statuses;

namespace SummerJam1.Units
{
    public class DestroyAfterTurns : EnabledWhenAtTopOfEncounterSlot, IDescription, IAmount
    {
        [JsonProperty] public int Amount { get; set; } = 1;

        [OnTurnEnded]
        private void OnTurnEnded()
        {
            Amount--;

            if (Amount <= 0)
            {
                Entity.Destroy();
            }
        }

        public string Description
        {
            get
            {
                if (Amount == 1)
                {
                    return "Expires after this turn.";
                }
                
                return $"Expires after {Amount} turns.";
            }
        }
    }
}
