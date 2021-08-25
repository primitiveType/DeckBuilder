using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data.Events
{
    public class DamageDealtArgs
    {
        public int ActorId { get; }
        public int HealthDamage { get; }
        public int TotalDamage { get; }
        public GameEntity Source { get; }

        public DamageDealtArgs(int actorId, int totalDamage, int healthDamage, GameEntity source)
        {
            ActorId = actorId;
            HealthDamage = healthDamage;
            Source = source;
            TotalDamage = totalDamage;
        }
    }
}