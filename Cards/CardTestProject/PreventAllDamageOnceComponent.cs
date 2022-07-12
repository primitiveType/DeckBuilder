using Api;
using CardsAndPiles;

namespace CardTestProject
{
    public class PreventAllDamageOnceComponent : Component
    {
        [OnRequestDamageMultipliers]
        private void OnTryDealDamage(object sender, RequestDamageMultipliersEventArgs args)
        {
            args.Multiplier.Add(0);
            Entity.RemoveComponent(this);
        }
    }
}
