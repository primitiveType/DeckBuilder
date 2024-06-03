using CardsAndPiles;

namespace SummerJam1.Cards
{
    public class Asleep : SummerJam1Component, IDisableAbilities
    {
        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            if (!Game.Battle.IsTopCard(Entity))
            {
                return;
            }

            Entity.RemoveComponent(this);
        }
    }
}
