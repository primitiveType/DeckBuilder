using System;

namespace Api.Components
{
    public class Health : Component, ITakesDamage
    {
        public int Amount { get; private set; } = 10;
        public int Max { get; private set; } = 10;

        public int DealDamage(int damage)
        {
            int result = Math.Max(0, Amount - damage);

            int diff = Amount - result;
            Amount = result;

            return diff;
        }

        public void SetHealth(int health)
        {
            Amount = health;
        }

        public void SetMax(int max)
        {
            Max = max;
        }
        
        
    }
}