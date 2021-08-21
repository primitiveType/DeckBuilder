using DeckbuilderLibrary.Data;

namespace Data
{
    public class Actor : GameEntity, IInternalActor
    {
        public int Health { get; internal set; }

        public int Armor { get; internal set; }


        void IInternalActor.TryDealDamage(GameEntity source, int damage, out int totalDamage, out int healthDamage)
        {
            int armorDamage = System.Math.Min(damage, Armor);
            Armor -= armorDamage;
            healthDamage = damage - armorDamage;
            healthDamage = System.Math.Min(healthDamage, Health);
            totalDamage = healthDamage + armorDamage;

            Health -= healthDamage;
            //do we want to clamp the output of damage dealt? add overkill damage as a param?
            ((IInternalGameEventHandler)Context.Events).InvokeDamageDealt(this,
                new DamageDealtArgs(Id, totalDamage, healthDamage, source));

            if (Health <= 0)
            {
                ((IInternalGameEventHandler)Context.Events).InvokeActorDied(this,
                    new ActorDiedEventArgs(this, source, source /*TODO*/));
            }
        }

        public void GainArmor(int amount)
        {
            Armor += amount;
        }

        public void ResetArmor()
        {
            Armor = 0;
        }
    }

    public interface IActor : IGameEntity
    {
        int Health { get; }
        int Armor { get; }
    }
    
    internal interface IInternalActor : IActor
    {
        void TryDealDamage(GameEntity source, int damage, out int totalDamage, out int healthDamage);
    }
}