namespace Data
{
    public class Actor : GameEntity
    {
        public int Health { get;  set; }
        public int Armor { get;  set; }
        
        internal void TryDealDamage(int damage, out int totalDamage, out int healthDamage)
        {
            int armorDamage = System.Math.Min(damage, Armor);
            Armor -= armorDamage;
            healthDamage = damage - armorDamage;
            healthDamage = System.Math.Min(healthDamage, Health);
            totalDamage = healthDamage + armorDamage;

            Health -= healthDamage;

            ((IInternalGameEventHandler)Context.Events).InvokeDamageDealt(this, new DamageDealtArgs(Id, totalDamage, healthDamage));
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
}