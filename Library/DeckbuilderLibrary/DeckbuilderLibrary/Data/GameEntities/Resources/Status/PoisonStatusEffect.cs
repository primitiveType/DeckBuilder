using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Resources.Status
{
    public class PoisonStatusEffect : StatusEffect<PoisonStatusEffect>
    {
        public override string Name { get; } = nameof(PoisonStatusEffect);


        protected override void TriggerEffect()
        {
            //We don't have the "owner" in the sense that we don't know who applied the poison at this point.
            //Could be an issue... or it might not be.

            var actor = (IActor)Owner;
            Context.TryDealDamage(this, actor, actor.Node, Amount);
        }
    }
}