using Api;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class ReducePlayerStealthAtStartOfCombat : EnabledWhenAtTopOfEncounterSlot, IStatusEffect, ITooltip, IAmount
    {
        public int Amount { get; set; }

        public string Tooltip => $"At start of battle, reduces the player's stealth by {Amount}";

        [OnBattleStarted]
        public void OnBattleStarted(object sender, BattleStartedEventArgs args)
        {
            Game.Player.Entity.GetComponent<Stealth>().TryUseStealth(Amount);
        }
    }
}
