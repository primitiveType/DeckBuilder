﻿using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using PropertyChanged;

namespace SummerJam1.Statuses
{
    public class GainArmorEveryTurn : SummerJam1Component, IStatusEffect, ITooltip, IAmount, IDescription
    {
        public int Amount { get; set; }

        public string Description => Tooltip;

        [DependsOn(nameof(Amount))] public string Tooltip => $"Gains {Amount} Armor every turn.";

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.GetOrAddComponent<Armor>().Amount += Amount;
        }
    }
}