using System;
using Api.Components;

namespace CardsAndPiles
{
    public class CardEvents : CardEventsBase
    {
        internal override void OnRequestDealDamage(RequestDealDamageEventArgs args)
        {
            base.OnRequestDealDamage(args);
            ITakesDamage comp = args.Target.GetComponent<ITakesDamage>();
            comp.DealDamage(CalculateDamage(args));
        }

        private int CalculateDamage(RequestDealDamageEventArgs args)
        {
            var totalMultiplier = 1f;
            foreach (var multiplier in args.Multiplier)
            {
                if (multiplier == 0f)
                {
                    totalMultiplier = multiplier;
                    break;
                }

                totalMultiplier += multiplier;
            }

            return (int)Math.Ceiling(args.Amount * totalMultiplier); //TODO: math
        }
    }
}