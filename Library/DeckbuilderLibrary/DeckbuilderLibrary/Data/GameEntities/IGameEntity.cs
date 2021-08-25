namespace DeckbuilderLibrary.Data.GameEntities
{
    public interface IGameEntity
    {
        int Id { get; }
        IContext Context { get; }
    }
}