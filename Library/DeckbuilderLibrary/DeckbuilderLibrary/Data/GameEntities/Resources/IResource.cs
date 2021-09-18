using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public interface IResource : IGameEntity
    {
        IGameEntity Owner { get; }
        int Amount { get; }
        void Add(int amount);
        void Subtract(int amount);
        void Set(int amount);
    }
}