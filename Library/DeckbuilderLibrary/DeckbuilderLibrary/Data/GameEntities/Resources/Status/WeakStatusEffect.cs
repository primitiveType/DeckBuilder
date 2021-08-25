using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.GameEntities.Resources.Status
{
    public class WeakStatusEffect : StatusEffect<WeakStatusEffect>
    {
        public override string Name { get; }

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.DamageAmountRequested += EventsOnDamageAmountRequested;
        }

        private void EventsOnDamageAmountRequested(object sender, RequestDamageAmountEventArgs args)
        {
            if (args.Owner == Owner)
            {
                args.AddModifier(new DamageAmountModifier{MultiplicativeModifier = -.25f});
            }
        }

        protected override void TriggerEffect()
        {
            //does nothing on trigger.
        }
    }
}