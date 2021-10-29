namespace DeckbuilderLibrary.Data
{
    public interface IGameEvents : IBattleEventHandler
    {
        event BattleStarted BattleStarted;
        event EntityCreated EntityCreated;
    }
}