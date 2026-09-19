using System.Collections.Generic;
using System.Linq;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class Targeting : SummerJam1Component
    {
        [JsonProperty] public bool Aoe { get; set; }
        [JsonProperty] public TargetingType? Type { get; set; }

        public List<ITakesDamage> GetTargets(ITakesDamage baseTarget)
        {
            if (Aoe)
            {
                return Game.Battle.MonsterSlots.Entity.GetComponentsInChildren<ITakesDamage>()
                    .ToList();
            }

            return new List<ITakesDamage>
            {
                baseTarget
            };
        }
    }
}
