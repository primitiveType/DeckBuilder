using CardsAndPiles;
using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class StarterUnit : Unit
    {
        protected override void Initialize()
        {
            base.Initialize();
            //This will screw with deserialization
            Entity.AddComponent<DamageIntent>().Amount = 6;
            Entity.AddComponent<TransformAfterTurns>();
        }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.AddComponent<DamageIntent>().Amount = 6;
        }
        
        
    }
    
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
        }
        
        
    }
}