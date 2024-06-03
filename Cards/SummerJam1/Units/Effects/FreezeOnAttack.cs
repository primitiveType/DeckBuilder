using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public class FreezeOnAttack : EnabledWhenAtTopOfEncounterSlot, ITooltip, IDescription, IAmount
    {
        public int Amount { get; set; } = 1;
        public string Description => Tooltip;

        public string Tooltip
        {
            get
            {
                if (Amount == 1)
                {
                    return "Ice Breath - when attacking, freezes a random card in your hand.";
                }

                return $"Ice Breath - when attacking, freezes {Amount} random cards in your hand.";
            }
        }


        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Entity)
            {
                for (int i = 0; i < Amount; i++)
                {
                    IEntity random = Game.Battle.Hand.GetRandomWithCondition((entity, _) => entity.GetComponent<Frozen>() == null);
                    random?.AddComponent<Frozen>();
                }
            }
        }
    }
}
