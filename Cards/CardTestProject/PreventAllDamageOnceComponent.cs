using Api;

namespace Tests
{
    public class PreventAllDamageOnceComponent : Component
    {
        [OnRequestDealDamage]
        private void OnTryDealDamage(object sender, RequestDealDamageEventArgs args)
        {
            args.Multiplier.Add(0);
            Parent.RemoveComponent(this);
        }
    }
}