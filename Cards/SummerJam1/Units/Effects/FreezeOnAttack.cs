using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public class FreezeOnAttack : EnabledWhenAtTopOfEncounterSlot, ITooltip, IDescription
    {
        public string Description => Tooltip;

        public string Tooltip => "Ice Breath - when attacking, freezes a random card in your hand.";


        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Entity)
            {
                IEntity random = Game.Battle.Hand.GetRandomWithCondition((entity, _) => entity.GetComponent<Frozen>() == null);
                random?.AddComponent<Frozen>();
            }
        }
    }
}