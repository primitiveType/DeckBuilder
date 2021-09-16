using ca.axoninteractive.Geometry.Hex;

namespace DeckbuilderLibrary.Data.GameEntities.Actors
{
    internal interface IInternalActor : IActor, IInternalCoordinateProperty
    {
        void TryDealDamage(GameEntity source, int damage, out int totalDamage, out int healthDamage);
    }
}