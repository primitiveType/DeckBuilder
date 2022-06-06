using CardsAndPiles.Components;

namespace SummerJam1.Units
{
    public class StarterUnit : Unit
    {
        protected override void Initialize()
        {
            base.Initialize();
            //This will screw with deserialization
            var health = Entity.AddComponent<Health>();
            health.SetMax(10);
            health.SetHealth(10);
        }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.AddComponent<DamageIntent>().Amount = 15;
        }
    }
}