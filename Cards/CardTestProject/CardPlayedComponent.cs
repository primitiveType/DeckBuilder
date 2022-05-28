using System.ComponentModel;
using JetBrains.Annotations;
using Component = Api.Component;

namespace Tests
{
    public class CardPlayedComponent : Component
    {
        public bool CardPlayed { get; private set; }

        [OnCardPlayed]
        private void OnCardPlayed()
        {
            CardPlayed = true;
        }
    }

    public class PreventAllDamageOnceComponent : Component
    {
        [OnRequestDealDamage]
        private void OnTryDealDamage(object sender, RequestDealDamageEventArgs args)
        {
            args.Multiplier.Add(0);
            Parent.RemoveComponent(this);
        }
    }

    public class CardDiscardedComponent : Component
    {
        public bool CardDiscarded { get; private set; }

        [OnCardDiscarded]
        private void OnCardDiscarded()
        {
            CardDiscarded = true;
        }
    }
}