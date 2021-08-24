namespace DeckbuilderLibrary.Data.GameEntities.Actors
{
    internal interface IInternalActor : IActor
    {
        void TryDealDamage(GameEntity source, int damage, out int totalDamage, out int healthDamage);
    }
}