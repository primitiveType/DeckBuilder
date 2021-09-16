using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public abstract class BattleData<TGraph> : BattleData where TGraph : HexGraph, new()
    {
        public new TGraph Graph => base.Graph as TGraph;

        protected override void Initialize()
        {
            base.Initialize();
            base.Graph = Context.CreateEntity<TGraph>();
        }
    }

    public abstract class BattleData : GameEntity
    {
        public HexGraph Graph { get; protected set; }

        public abstract void PrepareBattle(Actor player);

    }
}