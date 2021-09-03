using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;

namespace DeckbuilderLibrary.Data
{
    internal interface IInternalGameEvents : IGameEvents, IInternalBattleEventHandler
    {
        void SetBattle(Battle newBattle);
    }
}