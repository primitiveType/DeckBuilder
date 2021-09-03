// using System.Collections.Generic;
// using DeckbuilderLibrary.Data.GameEntities;
// using DeckbuilderLibrary.Data.GameEntities.Actors;
//
// namespace Content.Battles
// {
//     public class TwoEnemies : BattleData
//     {
//         public override List<Enemy> GetStartingEnemies()
//         {
//             return null;
//         }
//
//         public override BattleGraph GetBattleInfo(Actor player)
//         {
//             var graph = new ThreeColumnGraph();
//             graph.Left.Actor = player;
//             graph.Middle.Actor = Context.CreateActor<BasicEnemy>(100, 0);
//             graph.Right.Actor = Context.CreateActor<BasicEnemy>(100, 0);
//
//             return graph;
//         }
//     }
// }