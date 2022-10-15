﻿namespace SummerJam1
{
    public class EncounterBoosterPack : SummerJam1Component, IBuyable
    {
        public int Cost { get; set; }

        public void Buy()
        {
            Money wallet = Game.Player.Entity.GetOrAddComponent<Money>();
            if (wallet.Amount >= Cost)
            {
                wallet.Amount -= Cost;
                foreach (PrefabReference prefab in Entity.GetComponentsInChildren<PrefabReference>())
                {
                    PrefabReference dungeon = null;
                    Context.CreateEntity(Entity.Parent, child => dungeon = child.AddComponent<PrefabReference>());
                    dungeon.Prefab = prefab.Prefab;
                }
            }
        }
    }
}