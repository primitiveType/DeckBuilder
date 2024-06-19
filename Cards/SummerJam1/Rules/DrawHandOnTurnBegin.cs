using System.Linq;
using CardsAndPiles;

namespace SummerJam1.Rules
{

    public class BattleEndsWhenAllEnemiesDefeated : SummerJam1Component
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            var slot = Game.Battle.EncounterSlots;

            if (slot.Entity.Children.Count == 0 ||
                (slot.Entity.Children.Count == 1 && slot.Entity.Children.First().Id == args.Entity.Id))
            {
                Events.OnBattleEnded(new BattleEndedEventArgs(true));
            }
        }
    }

    public class DrawHandOnTurnBegin : SummerJam1Component
    {
        private int CardDraw => 5;

        [OnDrawPhaseBegan]
        private void OnDrawPhaseBegan()
        {
            Game game = Context.Root.GetComponent<Game>();

            for (int i = 0; i < CardDraw; i++)
            {
                game.Battle.BattleDeck.DrawCard(true);
            }
        }
    }

    public class DrawHandOnBattleBegin : SummerJam1Component
    {
        private int CardDraw => 5;

        [OnBattleStarted]
        private void OnDrawPhaseBegan()
        {
            Game game = Context.Root.GetComponent<Game>();

            for (int i = 0; i < CardDraw; i++)
            {
                game.Battle.BattleDeck.DrawCard(true);
            }
        }
    }
}