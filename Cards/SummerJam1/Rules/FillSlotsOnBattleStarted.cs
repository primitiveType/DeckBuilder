using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class FillSlotsOnBattleStarted : SummerJam1Component
    {
        [OnBattleStarted]
        private void OnBattleStarted()
        {
            Game game = Context.Root.GetComponent<Game>();
            foreach (Pile slot in game.Battle.GetEmptySlots())
            {
                game.Battle.EncounterDrawPile.DrawCardInto(slot.Entity);
            }

            foreach (Pile slot in game.Battle.EncounterSlotsUpcoming)
            {
                game.Battle.EncounterDrawPile.DrawCardInto(slot.Entity);
            }
        }
    }
}
