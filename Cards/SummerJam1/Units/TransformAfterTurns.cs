using CardsAndPiles;
using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class TransformAfterTurns : SummerJam1Component
    {
        [JsonProperty] public int TurnsRemaining { get; set; } = 1;

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            TurnsRemaining--;

            if (TurnsRemaining <= 0)
            {
                Entity.RemoveComponent(this);
            }

            Events.OnUnitTransformed(new UnitTransformedEventArgs(Entity));
        }
    }
}
