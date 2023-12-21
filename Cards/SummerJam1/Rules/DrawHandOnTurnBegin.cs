﻿using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class WaitForCardCostsBeats : SummerJam1Component
    {
        public int BeatCost => 1;

        [OnWaitForCard]
        private void OnWaitForCard(object sender, WaitForCardEventArgs args)
        {
            Game.Battle.BeatTracker.AdvanceBeats(BeatCost);
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
