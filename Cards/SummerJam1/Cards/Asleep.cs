using CardsAndPiles;

namespace SummerJam1.Cards
{
    public class Asleep : SummerJam1Component, IDisableAbilities
    {
        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            Entity.RemoveComponent(this);
            Entity.AddComponent<Aggro>();
        }
    }
}