using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class Regen : SummerJam1Component, IAmount, IStatusEffect
    {
        public int Amount { get; set; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.GetComponent<Health>().TryHeal(Amount, Entity);
            Amount--;
            if (Amount <= 0)
            {
                Entity.RemoveComponent(this);
            }
        }
    }

    public class GainArmorEveryTurn : SummerJam1Component, IStatusEffect, ITooltip, IAmount
    {
        [PropertyChanged.DependsOn(nameof(Amount))]
        public string Tooltip => $"Gains {Amount} Armor every turn.";
        public int Amount { get; set; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.GetOrAddComponent<Armor>().Amount += Amount;
        }
    }


    public class ReducePlayerStealthAtStartOfCombat : SummerJam1Component, IStatusEffect, ITooltip
    {
        public int Amount { get; set; }

        [OnBattleStarted]
        public void OnBattleStarted(object sender, BattleStartedEventArgs args)
        {
            Game.Player.TryUseStealth(Amount);
        }

        public string Tooltip => $"At start of battle, reduces the player's stealth by {Amount}";
    }
}
