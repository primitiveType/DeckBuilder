using Api;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class DamageUnitEqualToStealth : SummerJam1Component, IEffect
    {
        public bool DoEffect(IEntity target)
        {
            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(Game.Player.Entity.GetComponent<Stealth>().Amount, Entity);
            return true;
        }
    }
}
