using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data
{
    internal interface IInternalGameEvents : IGameEvents, IInternalBattleEventHandler
    {
        void SetBattle(Battle newBattle);
    }
}