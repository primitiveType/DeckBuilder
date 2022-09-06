﻿using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class DeathAddsMoneyToPlayer : SummerJam1Component, ITooltip
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                int amount = Entity.GetComponent<Money>().Amount;
                Game.Player.Entity.GetOrAddComponent<Money>().Amount += amount;
            }
        }

        public string Tooltip => "Drops money when destroyed.";
    }
}
