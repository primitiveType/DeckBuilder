namespace DeckbuilderLibrary.Data.GameEntities.Actors
{
    public interface IActor : IGameEntity
    {
        int Health { get; }
        int Armor { get; }
        Resources.Resources Resources { get; }
    }
}