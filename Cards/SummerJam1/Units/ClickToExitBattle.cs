using System.ComponentModel;
using Api;
using SummerJam1.Cards;

namespace SummerJam1.Units
{
    public class ClickToExitBattle : SummerJam1Component, IClickable
    {
        public void Click()
        {
            Logging.Log("Clicked to exit.");
            if (Game.Battle.IsTopCard(Entity))
            {
                Events.OnBattleEnded(new BattleEndedEventArgs(true));
                IEntity dungeon = Context.CreateEntity(Game.Dungeons, delegate(IEntity entity) { entity.AddComponent<ExterminationDungeonPile>(); });
                Game.StartBattle(dungeon.GetComponent<DungeonPile>());
            }
        }
    }

    public interface IClickable
    {
        void Click();
    }
}
