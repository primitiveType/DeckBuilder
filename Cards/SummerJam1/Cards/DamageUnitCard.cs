using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards
{
    public class DamageUnitCard : SummerJam1Card
    {
        [JsonProperty] public int DamageAmount { get; private set; }


        protected override bool PlayCard(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(DamageAmount, Entity);
            return true;
        }

        [JsonIgnore] public override string Description => $"Deal {DamageAmount} damage to target Unit.";
    }
}