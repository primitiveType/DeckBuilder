using System;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Terrain;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public class EmptyBattleData : BattleData<HexGraph>
    {
        public override void PrepareBattle(Actor player)
        {
        }
    }

    public class BasicBattleData : BattleData<HexGraph>
    {
        public override void PrepareBattle(Actor player)
        {
            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);

            if (Graph.TryGetNode(new AxialHexCoord(0, 2).ToCubic(), out var node1))
            {
                node1.TryAdd(enemy);
            }

            if (Graph.TryGetNode(new AxialHexCoord(0, 0).ToCubic(), out var node2))
            {
                node2.TryAdd(player);
            }
            // Graph.
            // Graph.Left.AddEntityNoEvent(player);
            // Graph.Right.AddEntityNoEvent(enemy);
        }
    }

    public class FunBattleData : BattleData<HexGraph>
    {
        public override void PrepareBattle(Actor player)
        {
            AddEnemy(0, 2);
            AddEnemy(0, 4);
            AddEnemy(6, 8);
            AddEnemy(4, 8);
            AddEnemy(8, 2);
            AddEnemy(8, 5);

            if (Graph.TryGetNode(new AxialHexCoord(0, 0).ToCubic(), out var node2))
            {
                node2.TryAdd(player);
            }

            var terrain = Context.CreateEntity<BlockedTerrain>();
            if (Graph.TryGetNode(new AxialHexCoord(4, 5).ToCubic(), out var node3))
            {
                if (node3.TryAdd(terrain))
                {
                }
                else
                {
                    throw new Exception("Failed to add terrain!");
                }
            }
        }

        private void AddEnemy(int x, int y)
        {
            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);
            AddActor(enemy, new AxialHexCoord(x, y));
        }

        private void AddActor(BasicEnemy enemy, AxialHexCoord coord)
        {
            if (Graph.TryGetNode(coord.ToCubic(), out var node1))
            {
                node1.TryAdd(enemy);
            }
        }
    }
}