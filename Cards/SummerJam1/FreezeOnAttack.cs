using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1
{
    public class FreezeOnAttack : EnabledWhenInEncounterSlot, ITooltip, IDescription
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
    
    public class GainStrengthWhenMoving : SummerJam1Component, IAmount, IDescription
    {
        public int Amount { get; set; }

        [OnCardMoved]
        private void OnCardMoved(object sender, CardMovedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                Entity.GetOrAddComponent<Strength>().Amount += Amount;
            }
        }

        public string Description => $"Gains {Amount} strength each time it moves.";
    }
    
    public class GainStrengthEachTurnWhenInUpcomingSlot : SummerJam1Component, IAmount, IDescription
    {
        public int Amount { get; set; }


        [OnTurnEnded]
        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            if (Entity.GetComponentInParent<UpcomingEncounterSlotPile>() != null)
            {
                Entity.GetOrAddComponent<Strength>().Amount += Amount;
            }
        }
        public string Description => $"Gains {Amount} Strength each turn spent above the board.";
    }

    public class BurnOnAttack : EnabledWhenInEncounterSlot, ITooltip, IDescription, IAmount
    {
        public string Description => Tooltip;


        public string Tooltip => $"Fire Breath - when attacking, adds {Amount} burn.";

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Entity)
            {
                args.EntityId.GetOrAddComponent<Burn>().Amount += Amount;
            }
        }

        public int Amount { get; set; } = 1;
    }
}
