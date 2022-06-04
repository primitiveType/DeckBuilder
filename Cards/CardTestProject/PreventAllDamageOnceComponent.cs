using Api;
using CardsAndPiles;

namespace CardTestProject
{
    public class PreventAllDamageOnceComponent : Component
    {
        [OnRequestDealDamage]
        private void OnTryDealDamage(object sender, RequestDealDamageEventArgs args)
        {
            args.Multiplier.Add(0);
            Entity.RemoveComponent(this);
        }
    }
}