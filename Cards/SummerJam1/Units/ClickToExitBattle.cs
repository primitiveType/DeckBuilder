using Api;

namespace SummerJam1.Units
{
    public abstract class ClickToExitBattle<TNextDungeonType> : SummerJam1Component, IClickable, IBottomCard
        where TNextDungeonType : DungeonPile, new()
    {
        public string DungeonList { get; private set; }

        public void Click()
        {
            Logging.Log("Clicked to exit.");
            if (Game.Battle.IsTopCard(Entity))
            {
                IEntity dungeon = Context.CreateEntity(Entity, delegate(IEntity entity)
                {
                    var dungeonPile = entity.AddComponent<TNextDungeonType>();
                    dungeonPile.DungeonList = DungeonList;
                });
                ExtraSetup(dungeon);
                Game.PrepareNextDungeon(dungeon);
                Events.OnBattleEnded(new BattleEndedEventArgs(true));
            }
        }

        protected virtual void ExtraSetup(IEntity dungeonPile)
        {
            //do nothing by default.
        }
    }
}
