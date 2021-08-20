public class DamageDealtArgs
{
    public int ActorId { get; }
    public int HealthDamage { get; }
    public int TotalDamage { get; }

    public DamageDealtArgs(int actorId, int totalDamage, int healthDamage)
    {
        ActorId = actorId;
        HealthDamage = healthDamage;
        TotalDamage = totalDamage;
    }
}