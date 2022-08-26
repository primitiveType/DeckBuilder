using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class FillSlotsOnTurnEnd : SummerJam1Component
    {
        [OnTurnBegan]
        private void OnDungeonPhaseEnded()
        {
            Game game = Context.Root.GetComponent<Game>();
            foreach (Pile slot in game.Battle.GetEmptySlots())
            {
                game.Battle.EncounterDrawPile.DrawCardInto(slot.Entity);
            }
        }
    }
}
