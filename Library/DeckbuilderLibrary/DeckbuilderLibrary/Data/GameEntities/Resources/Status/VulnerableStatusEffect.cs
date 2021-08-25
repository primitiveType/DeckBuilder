using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.GameEntities.Resources.Status
{
    public class VulnerableStatusEffect : StatusEffect<VulnerableStatusEffect>
    {
        public override string Name { get; }

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.DamageAmountRequested += EventsOnDamageAmountRequested;
        }

        private void EventsOnDamageAmountRequested(object sender, RequestDamageAmountEventArgs args)
        {
            if (args.Target == Owner)
            {
                args.AddModifier(new DamageAmountModifier{MultiplicativeModifier = .5f});
            }
        }

        protected override void TriggerEffect()
        {
            //does nothing on trigger.
        }
    }
}