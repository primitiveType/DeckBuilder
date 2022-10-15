using CardsAndPiles;

namespace SummerJam1.Cards
{
    public class Aggro : SummerJam1Component
    {
        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            if (!Game.Battle.IsTopCard(Entity))
            {
                Entity.AddComponent<Asleep>();
            }
        }
    }
}
