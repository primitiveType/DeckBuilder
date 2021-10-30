using Content.Cards;
using DeckbuilderLibrary.Data;

namespace Content.Collectible
{
    using DeckbuilderLibrary.Data.GameEntities.Terrain;

    public class CollectibleDiscoverCards : Collectible
    {
        protected override void OnCollected()
        {
            Context.Discover(new[] { typeof(Anger), typeof(Attack10DamageExhaust) }, PileType.DrawPile);
        }
    }
}